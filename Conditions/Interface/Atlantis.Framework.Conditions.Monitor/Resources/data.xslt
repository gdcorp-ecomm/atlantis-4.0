﻿<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:template match="/">
    <xsl:text disable-output-escaping='yes'>&lt;!DOCTYPE html></xsl:text>
    <html>
      <head>
        <title>Atlantis Framework Conditions Interface</title>
        <style type="text/css" media="screen">
          table { border: solid thin black; }
          th { border: solid thin black; }
          td { border: solid thin black; vertical-align: top; text-align: left; padding: 2px; }
        </style>
      </head>
      <body>
        <div>
          Machine Name: <xsl:value-of select="//conditionhandlers/@machinename"/><br />
          Process Id: <xsl:value-of select="//conditionhandlers/@processid"/><br />
          Conditions Interface Version: <xsl:value-of select="//conditionhandlers/@conditionsinterfaceversion"/><br />
        </div>
        <br/>
        <div>
          Registered Condition Handlers:
        </div>
        <div>
          <xsl:call-template name="ConditionHandlersTable">
            <xsl:with-param name="handlersList" select="//conditionhandlers/conditionhandler"></xsl:with-param>
          </xsl:call-template>
        </div>
      </body>
    </html>
  </xsl:template>

  <xsl:template name="ConditionHandlersTable">
    <xsl:param name="handlersList"></xsl:param>
    <table>
      <tr>
        <th>Condition Name</th>
        <th>Condition Handler</th>
        <th>Assembly</th>
        <th>Description</th>
        <th>Version</th>
      </tr>
      <xsl:for-each select="$handlersList/.">
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
            <xsl:value-of select="./@name"/>
          </td>
          <td>
            <xsl:value-of select="./@handler"/>
          </td>
          <td>
            <xsl:value-of select="./@assembly"/>
          </td>
          <td>
            <xsl:value-of select="./@description"/>
          </td>
          <td>
            <xsl:value-of select="./@version"/>
          </td>
        </tr>
      </xsl:for-each>
    </table>
  </xsl:template>
</xsl:stylesheet>