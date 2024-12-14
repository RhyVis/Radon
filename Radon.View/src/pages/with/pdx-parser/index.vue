<script lang="ts" setup>
import { TransformIcon } from "tdesign-icons-vue-next";
import { useI18n } from "vue-i18n";
import { useRouter } from "vue-router";

const { t } = useI18n();
const router = useRouter();
const toTree = () => router.push("pdx-parser/lang-tree");
const toEvent = () => router.push("pdx-parser/lang-event");
</script>

<template>
  <content-layout :subtitle="t('subtitle')" :title="t('title')">
    <div class="w-full">
      <t-text>{{ t("description") }}</t-text>
      <t-space class="w-full text-center" direction="vertical">
        <t-divider />
        <t-tag>{{ t("btn.tree") }}</t-tag>
        <t-button shape="square" @click="toTree">
          <TransformIcon />
        </t-button>
        <t-divider />
        <t-tag>{{ t("btn.event") }}</t-tag>
        <t-button shape="square" @click="toEvent">
          <TransformIcon />
        </t-button>
        <t-divider />
      </t-space>
    </div>
  </content-layout>
</template>

<i18n locale="en">
title: "PDX Parser"
subtitle: "Reverse of file structure"
description: "Parse Paradox Game language file into structure."
btn:
  tree: "Tree Parse"
  event: "Event Parse"
</i18n>

<i18n locale="zh-CN">
title: "PDX 解析器"
subtitle: "文件结构解析"
description: "解析 Paradox 游戏语言文件为结构。"
btn:
  tree: "树解析"
  event: "事件解析"
</i18n>
