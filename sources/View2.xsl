<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="3.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" indent="yes">
	<xsl:param name="selected"></xsl:param>
	<xsl:template match="employees">
		<html>

		<head>
			<meta charset="UTF-8" />
			<meta name="viewport" content="width=device-width, initial-scale=1.0" />
			<title><xsl:value-of select="$selected" /></title>
			<style>body{font-family:sans-serif}h1{color:#d2691e}table{border:5px solid #b8860b;background:#fff8dc}tr td:nth-child(1){font-size:18px;font-weight:bolder;color:#b8860b}tr td:nth-child(2) span:hover{background:#fffff0;display:block}hr:last-of-type{display:none}</style>
		</head>

		<body>
			<h1>Employee Information</h1>
			<xsl:for-each select="employee">
				<xsl:choose>
					<xsl:when test="name = $selected">
						<table border="2" cellspacing="2" cellpadding="10">
							<tr>
								<td>Name</td>
								<td>
									<xsl:value-of select="name" />
								</td>
							</tr>
							<tr>
								<td>Phones</td>
								<td>
									<xsl:for-each select="phones/phone">
										<span><b><xsl:value-of select="@type" />: </b><xsl:value-of select="." /></span>
										<hr />
									</xsl:for-each>
								</td>
							</tr>
							<tr>
								<td>Addresses</td>
								<td>
									<xsl:for-each select="addresses/address">
										<span><xsl:value-of select="street" />, <xsl:value-of select="building" />, <xsl:value-of select="region" />, <xsl:value-of select="city" />, <xsl:value-of select="country" /></span>
										<hr />
									</xsl:for-each>
								</td>
							</tr>
							<tr>
								<td>Email</td>
								<td>
									<xsl:value-of select="mail" />
								</td>
							</tr>
						</table>
					</xsl:when>
				</xsl:choose>
			</xsl:for-each>
		</body>

	</html>
</xsl:template>
</xsl:stylesheet>