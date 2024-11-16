<script lang="ts" setup>
import { fontLoaderKey } from "@/lib/symbol/loaderSymbols";
import { inject } from "vue";
import { useI18n } from "vue-i18n";

const fontLoader = inject(fontLoaderKey)!;
const { t } = useI18n();
const { current, completed, hasError } = fontLoader;
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
    <t-title level="h4">{{ t("version.tt") }}</t-title>
    <t-paragraph>
      <version-check />
    </t-paragraph>
    <t-paragraph>
      <t-title level="h6" :content="t('version.why')" />
      <t-text>{{ t("version.reason") }}</t-text>
    </t-paragraph>
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
version:
  tt: "Version Check"
  why: "Why version check?"
  reason: "Because I can't handle the server-side page cache settings, sometimes it works and sometimes it doesn't, I'm 🤗"
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
version:
  tt: "版本检查"
  why: "为什么会有版本检查？"
  reason: "因为我玩不转服务端页面缓存设置，有时候管用有时候又不管用，摆了🤗"
</i18n>
