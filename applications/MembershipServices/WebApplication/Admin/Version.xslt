<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:output method="html" omit-xml-declaration="yes" indent="no" encoding="utf-8" />

  <xsl:variable name="at"><![CDATA[@]]></xsl:variable>
  <xsl:variable name="dollars"><![CDATA[$]]></xsl:variable>


  <xsl:param name="modificationdate"></xsl:param>
  <xsl:param name="versionid"></xsl:param>
  <xsl:param name="modifiedby"></xsl:param>

  <xsl:template name="FormatDate">
    <xsl:param name="DateTime" />

    <xsl:variable name="justDate">
      <xsl:value-of select="substring-before($DateTime,'T')" />
    </xsl:variable>

    <xsl:variable name="year">
      <xsl:value-of select="substring-before($justDate, '-')" />
    </xsl:variable>

    <xsl:variable name="monthday">
      <xsl:value-of select="substring-after($justDate, '-')" />
    </xsl:variable>

    <xsl:variable name="month">
      <xsl:value-of select="substring-after($monthday, '-')" />
    </xsl:variable>

    <xsl:variable name="day">
      <xsl:value-of select="substring-before($monthday, '-')" />
    </xsl:variable>

    <xsl:value-of select="$month" />
    <xsl:value-of select="'/'"/>
    <xsl:value-of select="$day" />
    <xsl:value-of select="'/'"/>
    <xsl:value-of select="$year"/>

  </xsl:template>

  <xsl:template match="/nurse">

    <div class="wrapper-block frm-section clearfix">
      <strong>MODIFICATION DATE: </strong>
      <xsl:value-of select="$modificationdate" />
      <br />
      <strong>VERSION: </strong>
      <xsl:value-of select="$versionid" />
      <br />
      <strong>MODIFIED BY: </strong>
      <xsl:value-of select="$modifiedby" />
      <br />
    </div>

    <h3>PERSONAL DATA</h3>
    <div class="wrapper-block frm-section clearfix">
      <p>
        <strong>User Name:</strong>&#xA0;<xsl:value-of select="username"/><br />
        <strong>Email:</strong>&#xA0;<xsl:value-of select="email"/><br />
        <strong>Secondary Email:</strong>&#xA0;<xsl:value-of select="secondaryemail"/><br />
        <strong>First Name:</strong>&#xA0;<xsl:value-of select="firstName"/><br />
        <strong>Last Name:</strong>&#xA0;<xsl:value-of select="lastName"/><br />
        <strong>Middle Initial:</strong>&#xA0;<xsl:value-of select="initial"/><br />
        <strong>Gender:</strong>
        <xsl:choose>
          <xsl:when test="genderId='1'"> Female</xsl:when>
          <xsl:when test="genderId='2'"> Male</xsl:when>
          <xsl:when test="genderId='3'"> Transgender Female</xsl:when>
          <xsl:when test="genderId='4'"> Transgender Male</xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="genderId"/>
          </xsl:otherwise>
        </xsl:choose>
        <br />
        <strong>Nickname:</strong>&#xA0;<xsl:value-of select="nickname"/><br />
        <strong>Date of Birth:</strong>&#xA0;<xsl:call-template name="FormatDate">
          <xsl:with-param name="DateTime" select="birthDate"/>
        </xsl:call-template>
      </p>
    </div>

    <h3>ADDRESS DATA</h3>
    <div class="wrapper-block frm-section clearfix">
      <p>
        <xsl:apply-templates select="addresses"/>
      </p>
    </div>

    <h3>PHONES</h3>
    <div class="wrapper-block frm-section clearfix">
      <p>
        <xsl:apply-templates select="phones"/>
      </p>
    </div>

    <h3>COMMUNICATION OPTIONS</h3>
    <div class="wrapper-block frm-section clearfix">
      <p>
        <xsl:apply-templates select="communicationOptions"/>
      </p>
    </div>

    <h3>EMPLOYMENT DATA</h3>
    <div class="wrapper-block frm-section clearfix">
      <strong>Designation:</strong>
      <xsl:choose>
        <xsl:when test="nurseDesignationId='1'"> RN</xsl:when>
        <xsl:when test="nurseDesignationId='2'"> NP</xsl:when>
        <xsl:when test="nurseDesignationId='3'"> LPN</xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="nurseDesignationId"/>
        </xsl:otherwise>
      </xsl:choose>
      <br />

      <strong>Employment Status:</strong>
      <xsl:choose>
        <xsl:when test="employmentStatusId='1'"> Active</xsl:when>
        <xsl:when test="employmentStatusId='2'"> Inactive</xsl:when>
        <xsl:when test="employmentStatusId='3'"> LTD</xsl:when>
        <xsl:when test="employmentStatusId='4'"> LOA</xsl:when>
        <xsl:when test="employmentStatusId='5'"> Maternity Leave</xsl:when>
        <xsl:when test="employmentStatusId='6'"> Retired</xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="employmentStatusId"/>
        </xsl:otherwise>
      </xsl:choose>
      <br />

      <strong>Completed Membership Form:</strong>
      <xsl:choose>
        <xsl:when test="completedMembershipForm='false'"> No</xsl:when>
        <xsl:when test="completedMembershipForm='true'"> Yes</xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="completedMembershipForm"/>
        </xsl:otherwise>
      </xsl:choose>
      <br />

      <strong>Issued Membership Card:</strong>
      <xsl:choose>
        <xsl:when test="issuedMembershipCard='false'"> No</xsl:when>
        <xsl:when test="issuedMembershipCard='true'"> Yes</xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="issuedMembershipCard"/>
        </xsl:otherwise>
      </xsl:choose>
    </div>


    <h3>FACILITIES DATA</h3>
    <div class="wrapper-block frm-section clearfix">
      <p>
        <xsl:apply-templates select="facilities"/>
      </p>
    </div>

    <h3>BOARD AND/OR COMMITTEE POSITIONS</h3>
    <div class="wrapper-block frm-section clearfix">
      <xsl:apply-templates select="committeePositions"/>
    </div>
  </xsl:template>

  <xsl:template match="address">
    <strong>Address Line 1:</strong>&#xA0;<xsl:value-of select="line1"/><br />
    <strong>Address Line 2:</strong>&#xA0;<xsl:value-of select="line2"/><br />
    <strong>City:</strong>&#xA0;<xsl:value-of select="city"/><br />
    <strong>Province:</strong>&#xA0;<xsl:value-of select="provinceId"/><br />
    <strong>Postal Code:</strong>&#xA0;<xsl:value-of select="postalCode"/><br />
  </xsl:template>

  <xsl:template match="phone">
    <xsl:value-of select="phoneNumber"/>

    <xsl:choose>
      <xsl:when test="extension=''">
        <xsl:text disable-output-escaping="yes"></xsl:text>
      </xsl:when>
      <xsl:otherwise>
        Ext: <xsl:value-of select="extension"/>
      </xsl:otherwise>
    </xsl:choose>

    <xsl:choose>
      <xsl:when test="phoneTypeId='1'">
        <xsl:text disable-output-escaping="yes">(Home)</xsl:text>
      </xsl:when>
      <xsl:when test="phoneTypeId='2'">
        <xsl:text disable-output-escaping="yes">(Work)</xsl:text>
      </xsl:when>
      <xsl:when test="phoneTypeId='3'">
        <xsl:text disable-output-escaping="yes">(Cell)</xsl:text>
      </xsl:when>
      <xsl:when test="phoneTypeId='4'">
        <xsl:text disable-output-escaping="yes">(Fax)</xsl:text>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="phoneTypeId"/>
      </xsl:otherwise>
    </xsl:choose>
    <br />
  </xsl:template>

  <xsl:template match="facility">
    <strong>
      <xsl:choose>
        <xsl:when test="priority='1'"> Primary Facility</xsl:when>
        <xsl:when test="priority='2'"> Secondary Facility</xsl:when>
        <xsl:when test="priority='3'"> Tertiary Facility</xsl:when>
      </xsl:choose>
    </strong>
    <br />

    <strong>Facility:</strong>&#xA0;<xsl:value-of select="facilityName"/><br />
    <strong>Employment Type:</strong>&#xA0;<xsl:value-of select="employmentType"/>
    <br />
    <strong>Local Position:</strong>
    <xsl:apply-templates select="localPositions"/>
    <br />
    <strong>Table Officer Position(s):</strong>
    <xsl:apply-templates select="tableOfficerPositions"/>
    <br />
    <br />
  </xsl:template>


  <xsl:template match="communicationOption">
    <xsl:value-of select="communicationOptionName"/>
  </xsl:template>

  <xsl:template match="localPosition">
    <xsl:value-of select="position"/>
  </xsl:template>

  <xsl:template match="tableOfficerPosition">
    <xsl:value-of select="position"/>
  </xsl:template>

  <xsl:template match="committeePosition">

    <strong>
      <xsl:value-of select="committeeName"/>
      <xsl:if test="regionName != ''"> (<xsl:value-of select="regionName"/>)</xsl:if><xsl:if test="districtName != ''"> (<xsl:value-of select="districtName"/>)</xsl:if>:
    </strong>
    &#xA0;<xsl:value-of select="positionName"/><br/>

  </xsl:template>

</xsl:stylesheet>

