<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:template match="/">
    <xsl:text disable-output-escaping='yes'>&lt;!DOCTYPE html></xsl:text>
    <html>
      <head>
        <title>Active Tests</title>
        <style type="text/css" media="screen">
          table { border: solid thin black; }
          th { border: solid thin black; }
          td { border: solid thin black; vertical-align: top; text-align: left; padding: 2px; }
        </style>
      </head>
      <body>
        <div>
          Machine Name: <xsl:value-of select="//data/@machinename"/><br />
          Process Id: <xsl:value-of select="//data/@processid"/><br />
          Interface Version: <xsl:value-of select="//data/@splitproviderinterfaceversion"/><br />
          Assembly Version: <xsl:value-of select="//data/@splitproviderversion"/><br />
        </div>
        <br/>
        <div>
          Active Tests:
        </div>
        <div>
          <xsl:call-template name="ActiveTestsTable">
            <xsl:with-param name="testsList" select="//data/activetests/activetest"></xsl:with-param>
          </xsl:call-template>
        </div>
      </body>
    </html>
  </xsl:template>

  <xsl:template name="ActiveTestsTable">
    <xsl:param name="testsList"></xsl:param>
    <table>
      <tr>
        <th>Run Id</th>
        <th>Test Id</th>
        <th>Version Number</th>
        <th>Eligibility Rules</th>
        <th>Start Date</th>
      </tr>
      <xsl:for-each select="$testsList/.">
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
            <xsl:value-of select="./@runid"/>
          </td>
          <td>
            <xsl:value-of select="./@testid"/>
          </td>
          <td>
            <xsl:value-of select="./@versionnumber"/>
          </td>
          <td>
            <xsl:value-of select="./@eligibilityrules"/>
          </td>
          <td>
            <xsl:value-of select="./@startdate"/>
          </td>
        </tr>
      </xsl:for-each>
    </table>
  </xsl:template>
</xsl:stylesheet>
