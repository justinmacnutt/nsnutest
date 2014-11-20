<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
    <xsl:output method="html" omit-xml-declaration="yes" indent="no" />
    <xsl:include href="../../core/templates/NavCore.xslt"/>

    <!-- input parameters -->
    <xsl:param name="currentID"/>
    <xsl:param name="domainName"/>
    <xsl:param name="secureDomainName"/>
    <xsl:param name="sitePath"/>

	<!-- don't show the current page when it's a tier1 page -->
	<xsl:template match="/Pages">
		<xsl:if test="Page[@PageId=$currentID and @TierLevel &gt; 1]">
			<div id="ow_sitePath">
				<xsl:call-template name="crumb">
					<xsl:with-param name="page" select="Page[@PageId=$currentID]"/>
				</xsl:call-template>
			</div>
		</xsl:if>
	</xsl:template>

    <xsl:template name="crumb">
        <xsl:param name="page"/>
        <xsl:if test="$page/@ParentId!=0">
            <!-- recursively call parent crumb first -->
            <xsl:call-template name="crumb">
                <xsl:with-param name="page" select="/Pages/Page[@PageId=$page/@ParentId]"/>
            </xsl:call-template>
            <xsl:text disable-output-escaping="yes">  &gt;  </xsl:text>
        </xsl:if>
        <xsl:choose>
            <xsl:when test="$page/@PageId=$currentID">
                <span class="sitepath_current"><xsl:value-of select="$page/@NavText"/></span>
            </xsl:when>
            <xsl:otherwise>
                <a>
                    <xsl:call-template name="formatAddress"><xsl:with-param name="page" select="$page"/></xsl:call-template>
                    <xsl:value-of select="$page/@NavText"/>
                </a>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>

</xsl:stylesheet>

