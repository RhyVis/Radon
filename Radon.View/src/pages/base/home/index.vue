<script lang="ts" setup>
import { fontLoaderKey } from "@/lib/symbol/loaderSymbols";
import { useVersionStore } from "@/store/version.ts";
import { get } from "@vueuse/core";
import { storeToRefs } from "pinia";
import { LogoGithubIcon } from "tdesign-icons-vue-next";
import { inject, onMounted } from "vue";
import { useI18n } from "vue-i18n";

const fontLoader = inject(fontLoaderKey)!;
const version = useVersionStore();
const { t } = useI18n();
const { current, completed, hasError } = fontLoader;
const { needUpdate, initialized, badState } = storeToRefs(version);

const jumpToGithub = () => window.open("https://github.com/RhyVis/Radon", "_blank");

onMounted(() => {
  if (get(initialized) && get(badState)) {
    version.init();
  }
});
</script>

<template>
  <content-layout :title="t('title')">
    <t-title level="h4">{{ t("description.tt") }}</t-title>
    <t-paragraph>
      <t-text>{{ t("description.content") }}</t-text>
    </t-paragraph>

    <t-title level="h4">{{ t("hint.tt") }}</t-title>
    <t-paragraph>
      <t-text>{{ t("hint.font-hint") }}</t-text>
    </t-paragraph>

    <t-title level="h4">{{ t("loading-status.tt") }}</t-title>
    <t-paragraph>
      <loader-status
        name="font"
        :label="t('loading-status.font')"
        :current="current"
        :completed="completed"
        :has-error="hasError"
      />
    </t-paragraph>

    <t-title level="h4">{{ t("status.tt") }}</t-title>
    <!-- Prompts -->
    <t-space direction="vertical" :size="6">
      <t-space align="center" :size="6">
        <prompt-online />
        <prompt-server-available />
      </t-space>
      <prompt-version-client />
      <prompt-version-server />
    </t-space>
    <t-paragraph>
      <t-text v-if="needUpdate">{{ t("status.hint") }}</t-text>
    </t-paragraph>

    <template #actions>
      <t-button class="r-no-select" variant="text" theme="primary" shape="circle" @click="jumpToGithub">
        <LogoGithubIcon />
      </t-button>
    </template>
  </content-layout>
</template>

<i18n locale="en">
title: Home
description:
  tt: Description
  content: "I don't think anyone needs me to teach them how to use it"
hint:
  tt: Hint
  font-hint: "It is recommended to disable scripts and plugins that render fonts on this site,
        which will cause the application's custom font rendering image function to be abnormal."
loading-status:
  tt: Loading Status
  font: Font
status:
  tt: "Status"
  hint: "If prompted to update the version, wait for the pop-up prompt and update"
</i18n>

<i18n locale="zh-CN">
title: 主页
description:
  tt: 简介
  content: "不会还有人需要我教怎么用吧，不会吧不会吧"
hint:
  tt: 提示
  font-hint: "建议在本站禁用字体渲染的脚本与插件，会导致应用自定义字体渲染图像的功能异常。"
loading-status:
  tt: 加载状态
  font: 字体
status:
  tt: "状态"
  hint: "如果提示版本更新，等待弹窗提示并更新"
</i18n>
