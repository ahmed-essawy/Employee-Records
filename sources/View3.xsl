<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="3.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" indent="yes">
	<xsl:param name="selected"></xsl:param>
	<xsl:template match="employees">
		<html>
		<head>
			<meta charset="UTF-8" />
			<meta name="viewport" content="width=device-width, initial-scale=1.0" />
			<title><xsl:value-of select="$selected" /></title>
			<style>body{font-family:cursive}h1{margin:0}.container{margin:0 auto;width:600px;text-align:center;border:2px solid red;padding:20px;background:#eee}.container strong{width:20%;border:2px solid orange;font-weight:bolder}.container span{float:right;text-align:left;width:70%;border:2px solid purple}.container span i{color:red}.container span:hover{font-size:20px;background:#deb887;padding:6.5px 10px;color:#fff}.container span:hover i{color:#000}p:nth-child(4) span:hover{padding:5px 10px}.container strong,.container span{padding:8px 10px;display:inline-block;font-size:18px;margin:2px 0;background:#fff}p::after,div::after{content:"";clear:both;display:block}</style>
		</head>
		<body>
			<div class="container">
				<h1>Employee Information</h1>
				<xsl:for-each select="employee">
					<xsl:choose>
						<xsl:when test="name = $selected">
							<p>
								<strong>Name:</strong>
								<span><xsl:value-of select="name" /></span>
							</p>
							<p>
								<strong>Phones:</strong>
								<xsl:for-each select="phones/phone">
									<span><i><xsl:value-of select="@type" />: </i><xsl:value-of select="." /></span>
								</xsl:for-each>
							</p>
							<p>
								<strong>Addresses:</strong>
								<xsl:for-each select="addresses/address">
									<span><xsl:value-of select="street" />, <xsl:value-of select="building" />, <xsl:value-of select="region" />, <xsl:value-of select="city" />, <xsl:value-of select="country" /></span>
								</xsl:for-each>
							</p>
							<p>
								<strong>Email:</strong>
								<span><xsl:value-of select="mail" /></span>
							</p>
						</xsl:when>
					</xsl:choose>
				</xsl:for-each>
			</div>
		</body>
	</html>
</xsl:template>
</xsl:stylesheet>