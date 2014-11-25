﻿<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output encoding="ISO-8859-1" indent="yes" omit-xml-declaration="yes"/>
  <!-- Parametri -->
  <xsl:param name="Area"/>
  <xsl:param name="urlCorrente"/>
  <xsl:param name="controller"/>
  <xsl:param name="action"/>
  <xsl:param name="roles"/>
  <xsl:param name="contesto"/>


  <xsl:template match="root">


    
    
    <!--
    $urlCorrente: <xsl:value-of select ="$urlCorrente"/>
    $controller: <xsl:value-of select ="$controller"/>
    $action: <xsl:value-of select ="$action"/>
    $Area: <xsl:value-of select ="$Area"/>
    
    zzz: <xsl:value-of select ="contains(@url,$urlCorrente)"/>
    yyy: <xsl:value-of select ="contains($urlCorrente, @url)"/>
    
    -->
    <ul>


      <!--
        <xsl:apply-templates select="siteMap[contains($urlCorrente, @url )]"/>



        <xsl:if test="string-length(siteMap/siteMapNode[contains(@url ,$urlCorrente)]/@title)  &gt; 0 ">
                <xsl:apply-templates select="siteMap[siteMapNode[contains(@url,$urlCorrente)]]" />
        </xsl:if>
        -->


      <xsl:apply-templates select="siteMap[siteMapNode[(@controller = $controller) and  (@action = $action)]]" />



      <!--
        <xsl:if test="string-length(siteMap[contains(@url,$urlCorrente)]/@DescriptionMenuTop) &gt; 0">
          <xsl:apply-templates select="siteMap[contains(@url,$urlCorrente)]"/>
        </xsl:if>


        <xsl:if test="string-length(siteMapNode/siteMapNode[contains(@url,$urlCorrente)]/@DescriptionMenuTop) = 0">
          <xsl:apply-templates select="siteMapNode/siteMapNode[siteMapNode[contains(@url,$urlCorrente)]]" mode="LEVEL_1"/>
        </xsl:if>
-->

    </ul>

  </xsl:template>




  <xsl:template match="siteMap">

    <xsl:apply-templates select="siteMapNode" mode="LEVEL_2"/>

  </xsl:template>








  <xsl:template match="siteMapNode" mode="LEVEL_1">

    <!--- nodo padre -->
    <xsl:if test="string-length(@DescriptionMenuTop) &gt; 0">
      <!--  <xsl:variable name="Codes" select="@authorization"/>
       <xsl:if test="contains($roles, '-12345') or ((contains($roles, $Codes) or (@authorization='?' and string-length($roles) &gt; 0) or @authorization='*'))">
      <xsl:if test="contains($roles, '-12345') or (contains($roles, @authorization) )">-->
      <xsl:element name="li">
        <xsl:element name="a">
          <xsl:attribute name="href">
            <xsl:if test="$Area = ''" >
                <xsl:value-of select="$contesto"/>/<xsl:value-of select="@controller"/>/<xsl:value-of select="@action" />  
            </xsl:if>
            <xsl:if test="$Area != ''" >
              <xsl:value-of select="$contesto"/>/<xsl:value-of select="$Area" />/<xsl:value-of select="@controller"/>/<xsl:value-of select="@action" />
            </xsl:if>
          </xsl:attribute>
          <xsl:value-of select="@title"/>
        </xsl:element>
      </xsl:element>
      <!-- </xsl:if>-->


      <xsl:apply-templates select="./siteMapNode" mode ="LEVEL_2"/>


    </xsl:if>
  </xsl:template>





  <xsl:template match="siteMapNode" mode="LEVEL_2">

    <xsl:if test="@hide ='false' ">

      <xsl:element name="li">
        <xsl:element name="a">
          <!--       <xsl:attribute name ="data-role">button</xsl:attribute>
          <xsl:attribute name ="data-theme">e</xsl:attribute>
-->

          <xsl:attribute name="href">

            <xsl:if test="$Area = ''" >
              <xsl:value-of select="$contesto"/>/<xsl:value-of select="@controller"/>/<xsl:value-of select="@action" />
            </xsl:if>
            <xsl:if test="$Area != ''" >
              <xsl:value-of select="$contesto"/>/<xsl:value-of select="$Area" />/<xsl:value-of select="@controller"/>/<xsl:value-of select="@action" />
            </xsl:if>
            
           
          </xsl:attribute>
          <xsl:value-of select="@title"/>
        </xsl:element>
      </xsl:element>

    </xsl:if>

  </xsl:template>










</xsl:stylesheet>
