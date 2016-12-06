package com.proff.teamcity.configTransformations

import jetbrains.buildServer.agent.plugins.beans.PluginDescriptor
import jetbrains.buildServer.agent.runner.BuildServiceAdapter
import jetbrains.buildServer.agent.runner.ProgramCommandLine
import jetbrains.buildServer.agent.runner.SimpleProgramCommandLine
import java.util.*

class TransformBuildService(private val pluginDescriptor: PluginDescriptor) : BuildServiceAdapter() {
    override fun makeProgramCommandLine(): ProgramCommandLine {
        val env = HashMap(environmentVariables)
        val params = runnerParameters
        env[TransformConstants.SOURCE_CONFIG_KEY] = params[TransformConstants.SOURCE_CONFIG_KEY]
        env[TransformConstants.TARGET_CONFIG_KEY] = params[TransformConstants.TARGET_CONFIG_KEY]
        if (params[TransformConstants.VERBOSE_LOGGING_CONFIG_KEY] != null)
            env[TransformConstants.VERBOSE_LOGGING_CONFIG_KEY] = params[TransformConstants.VERBOSE_LOGGING_CONFIG_KEY]
        return SimpleProgramCommandLine(env, workingDirectory.absolutePath, pluginDescriptor.pluginRoot.absolutePath + "/ConfigTransformer/ConfigTransformer.exe", listOf())
    }
}