<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt"  xmlns:a="http://schemas.microsoft.com/2003/10/Serialization/Arrays" exclude-result-prefixes="msxsl">
  <xsl:output method="html" indent="yes" />

  <xsl:template match="/">
    <html xmlns="http://www.w3.org/1999/xhtml">
      <head id="Head1" runat="server">
        <title>Unit Test Results</title>
        <style type="text/css" media="screen">
          .success_True
          {
          color: Green;
          font-weight: bold;
          }
          .success_False
          {
          color: Red;
          font-weight: bold;
          }
          .success_Ignore
          {
          color: Gold;
          font-weight: bold;
          }
          .rounded
          {
          height: 16px;
          width: 16px;
          -moz-border-radius: 8px;
          border-radius: 8px;
          }
          .pass
          {
          background-color: Green;
          }
          .fail
          {
          background-color: Red;
          }
          .ignore
          {
          background-color: Gold;
          }
          table
          {
          border: solid thin black;
          }
          th
          {
          border-bottom: solid thin black;
          }
          td
          {
          border-right: solid thin black;
          }
          tfoot tr td
          {
          border-top: solid thin black;
          }
        </style>
      </head>
      <body>
        <div>
          <xsl:apply-templates select="//TestResults" />
        </div>
        <div>
          <xsl:apply-templates select="//TestResults" mode="summary"  />
        </div>
        <div style="padding-top:20px;">
          <xsl:apply-templates select="//ExtendedLogData" />
        </div>

      </body>
    </html>
  </xsl:template>

  <xsl:template match="//TestResults">
    <table cellpadding="2" cellspacing="0" border="0" width="98%">
      <tr>
        <th>

        </th>
        <th>
          Test Name
        </th>
        <th>
          Pass/Fail
        </th>
        <th>
          Results
        </th>
      </tr>
      <xsl:for-each select="./TestResult">
        <xsl:variable name="status">
          <xsl:choose>
            <xsl:when test="./Success/text() = 'false'">Fail</xsl:when>
            <xsl:when test="./Success/text() = 'true'">Pass</xsl:when>
            <xsl:otherwise>Ignore</xsl:otherwise>
          </xsl:choose>
        </xsl:variable>
        <tr>
          <xsl:attribute name="class">
            <xsl:choose>
              <xsl:when test="$status = 'Ignore'">
                <xsl:text>success_Ignore</xsl:text>
              </xsl:when>
              <xsl:when test="$status = 'Pass'">
                <xsl:text>success_True</xsl:text>
              </xsl:when>
              <xsl:otherwise>
                <xsl:text>success_False</xsl:text>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
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

          <td style="vertical-align: top;">
            <div>
              <xsl:attribute name="class">
                <xsl:choose>
                  <xsl:when test="$status = 'Ignore'">
                    <xsl:text>ignore rounded</xsl:text>
                  </xsl:when>
                  <xsl:when test="$status = 'Pass'">
                    <xsl:text>pass rounded</xsl:text>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:text>fail rounded</xsl:text>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:attribute>

            </div>
          </td>
          <td valign="top" align="left">
            <xsl:value-of select="./TestName/text()"/>
          </td>
          <td valign="top" align="center">
            <xsl:value-of select="$status"/>
          </td>
          <td valign="top" align="left">
            <xsl:value-of select="./Result/text()"/>
          </td>
        </tr>

      </xsl:for-each>
    </table>
  </xsl:template>


  <xsl:template match="//ExtendedLogData">
    <table cellpadding="2" cellspacing="0" border="0" width="98%">
      <tr>
        <th>
          Test Name
        </th>
        <th>
          Results
        </th>
      </tr>
      <xsl:for-each select="./LogItem">
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

          <td valign="top" align="left">
            <xsl:value-of select="./Key/text()"/>
          </td>

          <td valign="top" align="left">
            <xsl:for-each select="./Value/Entry">
              <xsl:value-of select="./text()"/>
              <br />
            </xsl:for-each>
          </td>
        </tr>

      </xsl:for-each>
    </table>
  </xsl:template>

  <xsl:template match="//TestResults" mode="summary">
    <table cellpadding="2" cellspacing="0" border="0" width="98%">
      <tbody>
        <tr>
          <td colspan="4" style="border-top:solid thin black;" align="left">
            <br />
            <br />
            <strong>Test Summary:</strong>
          </td>
        </tr>
      </tbody>
      <tfoot>
        <tr>
          <td width="25%" align="center">
            <strong>
              <xsl:value-of select="count(//TestResult)"  /> Tests Run
            </strong>
          </td>
          <td class="success_True" width="25%" align="center">
            <xsl:value-of select="count(//TestResult[Success ='true'])"  />   Tests Passed
          </td>
          <td class="success_False" width="25%" align="center">
            <xsl:value-of select="count(//TestResult[Success ='false'])"  />  Tests Failed
          </td>
          <td class="success_Ignore" width="25%" align="center">
            <xsl:value-of select="count(//TestResult[Success =  ''])"  />  Tests Ignored
          </td>
        </tr>
      </tfoot>

    </table>
  </xsl:template>

</xsl:stylesheet>
