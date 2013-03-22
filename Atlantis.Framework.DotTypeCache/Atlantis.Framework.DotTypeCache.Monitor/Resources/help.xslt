<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">

	<xsl:template match="/">
		<xsl:text disable-output-escaping='yes'>&lt;!DOCTYPE html></xsl:text>
		<html>
			<head>
				<title>Atlantis Framework DotTypeCache Help</title>
				<style type="text/css" media="screen">
					table { border: solid thin black; }
					th { border: solid thin black; }
					td { border: solid thin black; vertical-align: top; text-align: left; padding: 2px; }
				</style>
			</head>
			<body>
				<div>
					Process Id: <xsl:value-of select="//MonitorUrls/@ProcessId"/><br />
					Machine Name: <xsl:value-of select="//MonitorUrls/@MachineName"/><br />
					DotTypeCache Version: <xsl:value-of select="//MonitorUrls/@DotTypeCacheVersion"/><br />
					DotTypeCache Interface Version: <xsl:value-of select="//MonitorUrls/@DotTypeCacheInterfaceVersion"/><br />
				</div>
				<br/>
				<div>
					<b>Monitor Urls:</b></div>
				<br/>
        <div>
          <xsl:call-template name="MonitorUrlsTable">
            <xsl:with-param name="urlsList" select="//MonitorUrls/MonitorUrl"></xsl:with-param>
          </xsl:call-template>
        </div>
			</body>
		</html>
	</xsl:template>

	<xsl:template name="MonitorUrlsTable">
		<xsl:param name="urlsList"></xsl:param>
		<table>
			<tr>
				<th>Type</th>
				<th>Sample Url</th>
			</tr>
			<xsl:for-each select="$urlsList/.">
				<tr>
					<xsl:attribute name="style">
						<xsl:choose>
							<xsl:when test="(position() mod 2) != 1">
								<xsl:text>background-color: #CCC;</xsl:text>
							</xsl:when>
							<xsl:otherwise>
								<xsl:text>background-color: #FFF;</xsl:text>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>

					<td>
						<xsl:value-of select="./@type"/>
					</td>
					<td>
						<xsl:variable name="url"> 
							<xsl:value-of select="./@sampleurl"/>						
						</xsl:variable>
						
						<a href="{$url}">
							<xsl:value-of select="$url"/>						
						</a>						
					</td>
				</tr>
			</xsl:for-each>
		</table>
	</xsl:template>
</xsl:stylesheet>
