<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">

  <xsl:template match="/">
    <xsl:text disable-output-escaping='yes'>&lt;!DOCTYPE html></xsl:text>
    <html>
      <head>
        <title>Atlantis Framework Engine Firewall Test</title>
        <style type="text/css" media="screen">
          table { border: solid thin black; }
          th { border: solid thin black; }
          td { border: solid thin black; vertical-align: top; text-align: left; padding: 2px; }
        </style>
      </head>
      <body>
        <div>
          Machine Name: <xsl:value-of select="//ConfigElements/@machinename"/><br />
          Machine IP: <xsl:value-of select="//ConfigElements/@machineip"/><br />
        </div>
        <br />
        <div>Failures</div>
        <div>
          <xsl:call-template name="ConfigElementsTable">
            <xsl:with-param name="elementsList" select="//ConfigElements/ConfigElement[@resultcode != '0']"></xsl:with-param>
          </xsl:call-template>
        </div>
        <br />
        <br />
        <div>Success</div>
        <div>
          <xsl:call-template name="ConfigElementsTable">
            <xsl:with-param name="elementsList" select="//ConfigElements/ConfigElement[@resultcode = '0']"></xsl:with-param>
          </xsl:call-template>
        </div>
      </body>
    </html>
  </xsl:template>

  <xsl:template name="ConfigElementsTable">
    <xsl:param name="elementsList"></xsl:param>
    <table>
      <tr>
        <th>Type</th>
        <th>Name</th>
        <th>ServiceUrl</th>
        <th>IPAddress</th>
        <th>Port</th>
        <th>Result</th>
      </tr>
      <xsl:for-each select="$elementsList/.">
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
            <xsl:value-of select="./@requesttype"/>
          </td>
          <td>
            <xsl:value-of select="./@name"/>
          </td>
          <td>
            <xsl:value-of select="./serviceurl"/>
          </td>
          <td>
            <xsl:value-of select="./ipaddress"/>
          </td>
          <td>
            <xsl:value-of select="./port"/>
          </td>
          <td>
            <xsl:value-of select="./result"/>
          </td>
        </tr>

      </xsl:for-each>
    </table>
  </xsl:template>

  <xsl:template match="//ConfigElement">
    
  </xsl:template>

</xsl:stylesheet>
