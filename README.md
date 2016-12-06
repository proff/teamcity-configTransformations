# TeamCity Config Transformations Plugin

TeamCity Config Transformations plugin provides support of applying .net  [config transformations](https://msdn.microsoft.com/en-us/library/dd465326(v=vs.110).aspx)

# Compatibility

The plugin is compatible with [TeamCity](https://www.jetbrains.com/teamcity/download/) 9.1.x and greater.

# Requirements
.net framework 4.0 on agent

# Build

1. build visual studio solution *runner/ConfigTransformer.sln*
2. copy builded files to *plugin/configTransformations-agent/ConfigTransformer*
3. execute *mvn package* in *plugin* folder