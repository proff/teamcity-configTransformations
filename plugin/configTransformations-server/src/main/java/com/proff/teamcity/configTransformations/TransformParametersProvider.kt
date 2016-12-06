package com.proff.teamcity.configTransformations

class TransformParametersProvider {
    val sourceConfigKey: String
        get() = TransformConstants.SOURCE_CONFIG_KEY

    val targetConfigKey: String
        get() = TransformConstants.TARGET_CONFIG_KEY

    val verboseLoggingKey: String
        get() = TransformConstants.VERBOSE_LOGGING_CONFIG_KEY
}