<script setup lang="ts">
import ContentLayout from "@/layout/frame/ContentLayout.vue";
import CLinkN from "@/pages/base/credits/comps/CLinkN.vue";
import CLinkPri from "@/pages/base/credits/comps/CLinkPri.vue";
import { useGlobalStore } from "@/store/global";
import { HomeIcon } from "tdesign-icons-vue-next";
import { MessagePlugin } from "tdesign-vue-next";

const global = useGlobalStore();
const counterIncrement = () => {
  if (!global.authShow) {
    global.authShow = true;
    MessagePlugin.info("Something went wrong...");
  }
};
</script>

<template>
  <ContentLayout title="Credits">
    <t-paragraph>
      <t-title level="h4"> Radon </t-title>
      <t-text>
        This project was made possible with the help of various open-source or free-to-use libraries, tools, and
        resources.
      </t-text>
    </t-paragraph>
    <t-paragraph>
      <t-title level="h5"> Frontend </t-title>
      <ul>
        <li>
          Vue.js - Powered by Vue.js for building user interfaces (
          <c-link-pri href="https://mit-license.org">MIT License</c-link-pri>
          )
        </li>
        <li>
          TDesign - Design component library from TDesign (
          <c-link-pri href="https://mit-license.org">MIT License</c-link-pri>
          )
        </li>
        <li>Cloudflare - Free static resource cache</li>
        <li>https://img.r10086.com - Random anime pictures</li>
      </ul>
    </t-paragraph>
    <t-paragraph>
      <t-title level="h5"> Backend </t-title>
      <ul>
        <li>
          ASP .Net Core - Framework by Microsoft (
          <c-link-pri href="https://mit-license.org">MIT License</c-link-pri>
          )
        </li>
      </ul>
    </t-paragraph>
    <t-paragraph>
      <t-title level="h5"> Tools </t-title>
      <ul>
        <li>IntelliJ Rider & WebStorm - Development tool by JetBrains</li>
      </ul>
    </t-paragraph>
    <t-divider />
    <t-title level="h4"> Licence </t-title>
    <t-paragraph>
      <t-text>
        The pages you are viewing includes various components and resources licensed under open-source licenses:
      </t-text>
    </t-paragraph>
    <t-paragraph>
      <t-text>
        Most of the project code is licensed under the
        <c-link-pri href="https://www.gnu.org/licenses/gpl-3.0.txt">GPL 3.0 Licence</c-link-pri>
        , but with some contents which need clarification:
      </t-text>
    </t-paragraph>
    <t-paragraph>
      <ul>
        <li>
          Some fonts, used or provided by the pages, are licensed under the
          <c-link-pri href="https://scripts.sil.org/OFL">SIL Open Font 1.1 Licence</c-link-pri>
          :
          <ul>
            <li>
              <c-link-n href="https://fonts.google.com/specimen/Lilita+One/license">LilitaOne</c-link-n>
            </li>
            <li>
              <c-link-n href="https://github.com/Des-Magmeta/ChildFunSans-Fusion">ChildFunSansFusion-Z</c-link-n>
            </li>
            <li>
              <c-link-n href="https://github.com/welai/glow-sans">GlowSansSC</c-link-n>
            </li>
            <li>
              <c-link-n href="https://github.com/be5invis/Iosevka">Iosevka Rx(Custom font based on Iosevka)</c-link-n>
            </li>
          </ul>
          Other fonts have commercial restrictions, or unclear licence, which is only for personal non-commercial usage:
          <ul>
            <li>
              <c-link-n href="https://lets.fontworks.co.jp/fonts/218">YurukaStd</c-link-n>
            </li>
            <li>
              <c-link-n href="https://www.ssfonts.com/shows/27/43.html">SSFangTangTi</c-link-n>
            </li>
            <li>
              <c-link-n href="https://www.typebank.co.jp">RoGSanSrfStd</c-link-n>
            </li>
          </ul>
        </li>
        <li>
          Other assets, such as images and other media, are licensed under the
          <c-link-pri href="https://creativecommons.org/licenses/by-sa/4.0/">CC BY-SA 4.0 Licence</c-link-pri>
        </li>
      </ul>
    </t-paragraph>
    <template #actions>
      <t-button
        class="r-no-select"
        variant="text"
        theme="primary"
        shape="circle"
        size="small"
        @click="counterIncrement"
      >
        <HomeIcon />
      </t-button>
    </template>
  </ContentLayout>
</template>
