<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<xsl:output method="html" omit-xml-declaration="yes" indent="yes" />
    <xsl:include href="../../core/templates/NavCore.xslt"/>

<!-- input parameters -->
<xsl:param name="currentID"/>
<xsl:param name="domainName"/>
<xsl:param name="secureDomainName"/>
<xsl:param name="sitePath"/>

<!-- determine the top-level page id -->
<xsl:variable name="tier1Id"><xsl:choose>
	<xsl:when test="/Pages/Page[@PageId=$currentID]/@TierLevel=1"><xsl:value-of select="$currentID"/></xsl:when>
	<xsl:otherwise><xsl:value-of select="/Pages/Page[@PageId=$currentID]/preceding-sibling::Page[@TierLevel=1][1]/@PageId"/></xsl:otherwise>
</xsl:choose></xsl:variable>

<xsl:variable name="tier2Id"><xsl:choose>
	<xsl:when test="/Pages/Page[@PageId=$currentID]/@TierLevel=1"></xsl:when>
	<xsl:when test="/Pages/Page[@PageId=$currentID]/@TierLevel=2"><xsl:value-of select="$currentID"/></xsl:when>
	<xsl:otherwise><xsl:value-of select="/Pages/Page[@PageId=$currentID]/preceding-sibling::Page[@TierLevel=2][1]/@PageId"/></xsl:otherwise>
</xsl:choose></xsl:variable>

<xsl:variable name="tier3Id"><xsl:choose>
	<xsl:when test="/Pages/Page[@PageId=$currentID]/@TierLevel=1"></xsl:when>
	<xsl:when test="/Pages/Page[@PageId=$currentID]/@TierLevel=2"></xsl:when>
	<xsl:when test="/Pages/Page[@PageId=$currentID]/@TierLevel=3"><xsl:value-of select="$currentID"/></xsl:when>
	<xsl:otherwise><xsl:value-of select="/Pages/Page[@PageId=$currentID]/preceding-sibling::Page[@TierLevel=3][1]/@PageId"/></xsl:otherwise>
</xsl:choose></xsl:variable>

<xsl:template match="/Pages">
	<xsl:if test="Page[@PageId=$tier2Id] and Page[@TierLevel=3 and @ParentId=$tier2Id and not(contains(@Hidden, $navigation) or contains(@HiddenByParent,$navigation) or @Locked='true' or @LockedByParent='true')]">
	<div id="ow_secoNav">
		<ul class="ow_secoNavList">
		<xsl:apply-templates select="Page[@TierLevel=3 and @ParentId=$tier2Id and not(contains(@Hidden, $navigation) or contains(@HiddenByParent,$navigation) or @Locked='true' or @LockedByParent='true')]"/>
		</ul>
	</div>
	</xsl:if>
</xsl:template>

<xsl:template match="Page[@TierLevel=3]">
	<li><a>
		<xsl:call-template name="formatAddress"/>
		<xsl:if test="@PageId=$tier3Id"><xsl:attribute name="class">current</xsl:attribute></xsl:if>
		<xsl:value-of select="@NavText"/>
	</a>
	<xsl:if test="./@PageId=$tier3Id and parent::*/Page[@ParentId=$tier3Id and not(contains(@Hidden, $navigation) or contains(@HiddenByParent,$navigation) or @Locked='true' or @LockedByParent='true')]">
	<ul>
		<xsl:apply-templates select="parent::*/Page[@TierLevel=4 and @ParentId=$tier3Id and not(contains(@Hidden, $navigation) or contains(@HiddenByParent,$navigation) or @Locked='true' or @LockedByParent='true')]"/>
	</ul>
	</xsl:if>
	</li>
</xsl:template>

<xsl:template match="Page[@TierLevel=4]">
	<li><a>
		<xsl:call-template name="formatAddress"/>
		<xsl:if test="@PageId=$currentID"><xsl:attribute name="class">current</xsl:attribute></xsl:if>
		<xsl:value-of select="@NavText"/>
	</a></li>
</xsl:template>

</xsl:stylesheet>
