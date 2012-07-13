<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt"  xmlns:a="http://schemas.microsoft.com/2003/10/Serialization/Arrays" exclude-result-prefixes="msxsl">

  <xsl:template match="/">
    <xsl:text disable-output-escaping='yes'>&lt;!DOCTYPE html></xsl:text>
    <html>
      <head>
        <title>Atlantis Framework Engine Statistics</title>
        <style type="text/css" media="screen">
          table { border: solid thin black; }
          th { border-bottom: solid thin black; }
          td { border-right: solid thin black; vertical-align: top; text-align: left; }
          tfoot tr td { border-top: solid thin black; }
        </style>
      </head>
      <body>
        <div>
          Machine Name: <xsl:value-of select="//ConfigElements/@machinename"/><br />
          Process Id: <xsl:value-of select="//ConfigElements/@processid"/><br />
        </div>
        <br />
        <div>
          <xsl:apply-templates select="//ConfigElements" />
        </div>
      </body>
    </html>
  </xsl:template>

  <xsl:template match="//ConfigElements">
    <table cellpadding="2" cellspacing="0" border="0" width="98%">
      <tr>
        <th>Type</th>
        <th>Handler</th>
        <th>Assembly</th>
        <th>Description</th>
        <th>Version</th>
        <th>Calls/min</th>
        <th>Success</th>
        <th>Fail</th>
        <th>Fail%</th>
        <th>Avg Success (ms)</th>
        <th>Avg Fail (ms)</th>
        <th>TimeFrame (min)</th>
      </tr>
      <xsl:for-each select="./ConfigElement">
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
            <xsl:value-of select="./@requesthandler"/>
          </td>
          <td>
            <xsl:value-of select="./@assembly"/>
          </td>
          <td>
            <xsl:value-of select="./@assemblydescription"/>
          </td>
          <td>
            <xsl:value-of select="./@assemblyfileversion"/>
          </td>
          <td>
            <xsl:value-of select="./@callsperminute"/>
          </td>
          <td>
            <xsl:value-of select="./@succeeded"/>
          </td>
          <td>
            <xsl:value-of select="./@failed"/>
          </td>
          <td>
            <xsl:value-of select="./@failurerate"/>
          </td>
          <td>
            <xsl:value-of select="./@avgsuccessms"/>
          </td>
          <td>
            <xsl:value-of select="./@avgfailms"/>
          </td>
          <td>
            <xsl:value-of select="./@runminutes"/>
          </td>
        </tr>

      </xsl:for-each>
    </table>
  </xsl:template>

</xsl:stylesheet>
