<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<xsl:output method="html" omit-xml-declaration="yes" indent="yes" />
	<xsl:include href="../../core/templates/NavCore.xslt"/>

<xsl:param name="currentID"/>
<xsl:param name="domainName"/>
<xsl:param name="secureDomainName"/>
<xsl:param name="sitePath"/>

<!-- determine the root page id as the first page in display order not hidden -->
<xsl:variable name="tier1Id"><xsl:call-template name="getHomePageId"/></xsl:variable>

<xsl:variable name="tier2Id"><xsl:choose>
	<xsl:when test="/Pages/Page[@PageId=$currentID]/@TierLevel=1"></xsl:when>
	<xsl:when test="/Pages/Page[@PageId=$currentID][@TierLevel=2][@ParentId=$tier1Id]"><xsl:value-of select="$currentID"/></xsl:when>
	<xsl:when test="/Pages/Page[@PageId=$currentID]/preceding-sibling::Page[@TierLevel=1][1]/@PageId=$tier1Id"><xsl:value-of select="/Pages/Page[@PageId=$currentID]/preceding-sibling::Page[@TierLevel=2][1]/@PageId"/></xsl:when>
	<xsl:otherwise></xsl:otherwise>
</xsl:choose></xsl:variable>

<xsl:template match="/Pages">
<div id="ow_mainNav"><ul id="ow_mainNavList">
   <xsl:apply-templates select="Page[@TierLevel=2 and @ParentId=$tier1Id and not(contains(@Hidden, $navigation) or contains(@HiddenByParent,$navigation) or @Locked='true' or @LockedByParent='true')]"/>
</ul></div>
</xsl:template>

<xsl:template match="Page">
	<li><a class="ow_mainNav">
		<xsl:attribute name="id"><xsl:value-of select="concat('ow_mainNav_',@PageId)"/></xsl:attribute>
		<xsl:call-template name="formatAddress"/>
		<xsl:if test="@PageId=$tier2Id"><xsl:attribute name="class">ow_mainNav current</xsl:attribute></xsl:if>
		<xsl:value-of select="@NavText"/>
	</a></li>
</xsl:template>

</xsl:stylesheet>
