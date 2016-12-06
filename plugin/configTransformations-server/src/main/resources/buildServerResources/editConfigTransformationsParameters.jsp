<%@ taglib prefix="props" tagdir="/WEB-INF/tags/props" %>
<%@ taglib prefix="bs" tagdir="/WEB-INF/tags" %>
<jsp:useBean id="params" class="com.proff.teamcity.configTransformations.TransformParametersProvider"/>
<tr>
    <th><label for="${params.sourceConfigKey}">Source:</label></th>
    <td>
        <div class="posRel">
            <props:textProperty name="${params.sourceConfigKey}" className="longField" expandable="true"/>
        </div>
        <span class="error" id="error_${params.sourceConfigKey}"></span>
        <span class="smallNote">New line separated paths or transform file content.</span>
    </td>
</tr>
<tr>
    <th><label for="${params.targetConfigKey}">Target:</label></th>
    <td>
        <div class="posRel">
            <props:textProperty name="${params.targetConfigKey}" className="longField" expandable="true"/>
        </div>
        <span class="error" id="error_${params.targetConfigKey}"></span>
        <span class="smallNote">new line separated paths to files to be transformed.</span>
    </td>
</tr>
<tr>
    <th><label for="${params.verboseLoggingKey}">verbose logging:</label></th>
    <td>
        <props:checkboxProperty name="${params.verboseLoggingKey}"/>
    </td>
</tr>
