<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
    <xsl:output method="html" omit-xml-declaration="yes" indent="no" />
    <!-- input parameters -->
    <xsl:param name="currentID"/>
    <xsl:param name="domainName"/>
    <xsl:param name="secureDomainName"/>
    <xsl:param name="sitePath"/>

    <xsl:variable name="tier2Id">
        <xsl:choose>
            <xsl:when test="/Pages/Page[@PageId=$currentID]/@TierLevel=1"></xsl:when>
            <xsl:when test="/Pages/Page[@PageId=$currentID]/@TierLevel=2">
                <xsl:value-of select="$currentID"/>
            </xsl:when>
            <xsl:otherwise>
                <xsl:value-of select="/Pages/Page[@PageId=$currentID]/preceding-sibling::Page[@TierLevel=2][1]/@PageId"/>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:variable>
    <xsl:template match="/Pages">
        <span id="backgroundClass">
            <xsl:attribute name="class">
                <xsl:value-of select="Page[@PageId=$tier2Id]/@Name"/>
            </xsl:attribute>
        </span>
    </xsl:template>
</xsl:stylesheet>
