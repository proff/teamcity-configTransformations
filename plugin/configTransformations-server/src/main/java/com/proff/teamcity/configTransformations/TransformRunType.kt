package com.proff.teamcity.configTransformations

import jetbrains.buildServer.requirements.Requirement
import jetbrains.buildServer.requirements.RequirementType
import jetbrains.buildServer.serverSide.PropertiesProcessor
import jetbrains.buildServer.serverSide.RunType
import jetbrains.buildServer.serverSide.RunTypeRegistry
import jetbrains.buildServer.web.openapi.PluginDescriptor

class TransformRunType(private val pluginDescriptor: PluginDescriptor,
                       runTypeRegistry: RunTypeRegistry) : RunType() {

    init {
        runTypeRegistry.registerRunType(this)
    }

    override fun getViewRunnerParamsJspFilePath(): String? {
        return pluginDescriptor.getPluginResourcesPath("viewConfigTransformationsParameters.jsp")
    }

    override fun getType(): String {
        return TransformConstants.RUNNER_TYPE
    }

    override fun getDisplayName(): String {
        return TransformConstants.RUNNER_DISPLAY_NAME
    }

    override fun getEditRunnerParamsJspFilePath(): String? {
        return pluginDescriptor.getPluginResourcesPath("editConfigTransformationsParameters.jsp")
    }

    override fun getRunnerPropertiesProcessor(): PropertiesProcessor? {
        return PropertiesProcessor { emptyList() }
    }

    override fun getDescription(): String {
        return TransformConstants.RUNNER_DESCRIPTION
    }

    override fun getDefaultRunnerProperties(): Map<String, String> {
        return emptyMap()
    }

    override fun getRunnerSpecificRequirements(runParameters: MutableMap<String, String>): MutableList<Requirement> {
        return mutableListOf(Requirement("Exists=>DotNetFramework(4\\.0|4\\.5|4\\.5\\.1|4\\.5\\.2|4\\.6|4\\.6\\.1|4\\.6\\.2).*", null, RequirementType.EXISTS))
    }
}