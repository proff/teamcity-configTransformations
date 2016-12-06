<%@ taglib prefix="c" uri="http://java.sun.com/jsp/jstl/core" %>
<%@ taglib prefix="props" tagdir="/WEB-INF/tags/props" %>
<jsp:useBean id="propertiesBean" scope="request" type="jetbrains.buildServer.controllers.BasePropertiesBean"/>
<jsp:useBean id="params" class="com.proff.teamcity.configTransformations.TransformParametersProvider"/>
<c:if test="${not empty propertiesBean.properties[params.sourceConfigKey]}">
    <div class="parameter">
        Source: <props:displayValue name="${params.sourceConfigKey}"/>
    </div>
</c:if>
<c:if test="${not empty propertiesBean.properties[params.targetConfigKey]}">
    <div class="parameter">
        Target: <props:displayValue name="${params.targetConfigKey}"/>
    </div>
</c:if>
<c:if test="${not empty propertiesBean.properties[params.verboseLoggingKey]}">
    <div class="parameter">
        Verbose logging: <props:displayValue name="${params.verboseLoggingKey}"/>
    </div>
</c:if>
