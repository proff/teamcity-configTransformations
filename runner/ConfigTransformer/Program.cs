using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using JetBrains.TeamCity.ServiceMessages.Write.Special;
using Microsoft.Web.XmlTransform;

namespace ConfigTransformer
{
    internal class Program
    {
        private static void Main()
        {
            Console.OutputEncoding = Encoding.Default;

            var source = Environment.GetEnvironmentVariable("config-transformations-source");
            var target = Environment.GetEnvironmentVariable("config-transformations-target");
            var verbose = Environment.GetEnvironmentVariable("config-transformations-verbose") == "true";

            Console.WriteLine("source:");
            Console.WriteLine(source);

            Console.WriteLine("target:");
            Console.WriteLine(target);

            Console.WriteLine($"verbose: {verbose}");

            if (string.IsNullOrWhiteSpace(source) || string.IsNullOrWhiteSpace(target))
            {
                Console.WriteLine("warning: nothing to transform");
                return;
            }

            var sourceIsXml = false;
            try
            {
                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                XDocument.Parse(source);
                sourceIsXml = true;
            }
            catch (XmlException)
            {
            }

            var transformFiles = new List<string>();
            if (sourceIsXml)
                transformFiles.Add(source);
            else
                foreach (var s in source.Split(new[] {'\n'}, StringSplitOptions.RemoveEmptyEntries))
                    transformFiles.Add(s.Trim());

            var targetFiles = new List<string>();
            foreach (var s in target.Split(new[] {'\n'}, StringSplitOptions.RemoveEmptyEntries))
                targetFiles.Add(s.Trim());

            var logger = new TeamCityServiceMessages().CreateWriter();
            foreach (var transformFile in transformFiles)
            foreach (var targetFile in targetFiles)
            {
                var t = sourceIsXml ? "transform" : transformFile;
                using (var block = logger.OpenBlock($"apply {t} to {targetFile}"))
                {
                    var doc = new XmlTransformableDocument();
                    doc.Load(targetFile);

                    using (var l = new Logger(block, verbose))
                    {
                        var xt = new XmlTransformation(transformFile, !sourceIsXml, l);
                        xt.Apply(doc);
                    }
                    doc.Save(targetFile);
                }
            }
        }

        private class Logger : IXmlTransformationLogger, IDisposable
        {
            private readonly List<Lazy<ITeamCityWriter>> _blocks = new List<Lazy<ITeamCityWriter>>();
            private readonly ITeamCityWriter _logger;
            private readonly bool _verbose;

            public Logger(ITeamCityWriter logger, bool verbose)
            {
                _logger = logger;
                _verbose = verbose;
            }

            private ITeamCityWriter CurrentLogger
            {
                get
                {
                    if (_blocks.Count == 0)
                        return _logger;
                    return _blocks[_blocks.Count - 1].Value;
                }
            }

            public void Dispose()
            {
                for (var i = _blocks.Count - 1; i >= 0; i--)
                {
                    if(_blocks[i].IsValueCreated)
                        _blocks[i].Value.Dispose();
                }
                _blocks.Clear();
            }

            public void LogMessage(string message, params object[] messageArgs)
            {
                Message(MessageType.Normal, message, messageArgs);
            }

            public void LogMessage(MessageType type, string message, params object[] messageArgs)
            {
                Message(type, message, messageArgs);
            }

            public void LogWarning(string message, params object[] messageArgs)
            {
                Warning(string.Empty, 0, 0, message, messageArgs);
            }

            public void LogWarning(string file, string message, params object[] messageArgs)
            {
                Warning(file, 0, 0, message, messageArgs);
            }

            public void LogWarning(string file, int lineNumber, int linePosition, string message,
                params object[] messageArgs)
            {
                Warning(file, lineNumber, linePosition, message, messageArgs);
            }

            public void LogError(string message, params object[] messageArgs)
            {
                Error(string.Empty, 0, 0, message, messageArgs);
            }

            public void LogError(string file, string message, params object[] messageArgs)
            {
                Error(file, 0, 0, message, messageArgs);
            }

            public void LogError(string file, int lineNumber, int linePosition, string message,
                params object[] messageArgs)
            {
                Error(file, lineNumber, linePosition, message, messageArgs);
            }

            public void LogErrorFromException(Exception ex)
            {
                Exception(ex, string.Empty, 0, 0);
            }

            public void LogErrorFromException(Exception ex, string file)
            {
                Exception(ex, file, 0, 0);
            }

            public void LogErrorFromException(Exception ex, string file, int lineNumber, int linePosition)
            {
                Exception(ex, file, lineNumber, linePosition);
            }

            public void StartSection(string message, params object[] messageArgs)
            {
                Start(message, messageArgs);
            }

            public void StartSection(MessageType type, string message, params object[] messageArgs)
            {
                Start(message, messageArgs);
            }

            public void EndSection(string message, params object[] messageArgs)
            {
                End(message, messageArgs);
            }

            public void EndSection(MessageType type, string message, params object[] messageArgs)
            {
                End(message, messageArgs);
            }

            private void Message(MessageType type, string message, object[] messageArgs)
            {
                if(!_verbose && type==MessageType.Verbose)
                    return;
                CurrentLogger.WriteMessage(string.Format(message, messageArgs));
            }

            private void Warning(string file, int lineNumber, int linePosition, string message,
                object[] messageArgs)
            {
                CurrentLogger.WriteWarning(string.Format($"{file}:{lineNumber},{linePosition} - {message}", messageArgs));
            }

            private void Error(string file, int lineNumber, int linePosition, string message,
                object[] messageArgs)
            {
                CurrentLogger.WriteError(string.Format($"{file}:{lineNumber},{linePosition} - {message}", messageArgs));
            }

            private void Exception(Exception ex, string file, int lineNumber, int linePosition)
            {
                CurrentLogger.WriteError($"error: {file}:{lineNumber},{linePosition} {ex}");
            }

            private void Start(string message, object[] messageArgs)
            {
                var currentLogger = CurrentLogger;
                _blocks.Add(new Lazy<ITeamCityWriter>(() => currentLogger.OpenBlock(string.Format(message, messageArgs))));
            }

            private void End(string message, object[] messageArgs)
            {
                if (_blocks.Count > 0)
                {
                    var last = _blocks[_blocks.Count - 1];
                    if (last.IsValueCreated)
                    {
                        CurrentLogger.WriteMessage(string.Format(message, messageArgs));
                        last.Value.Dispose();
                    }
                    _blocks.RemoveAt(_blocks.Count - 1);
                }
            }
        }
    }
}