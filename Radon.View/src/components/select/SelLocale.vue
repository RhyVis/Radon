<script setup lang="ts">
import { useGlobalStore } from "@/store/global";
import { EarthIcon } from "tdesign-icons-vue-next";
import { MessagePlugin } from "tdesign-vue-next";
import { watch } from "vue";

const i18n = useI18n();
const global = useGlobalStore();

const options = [
  { content: "English", value: "en" },
  { content: "简体中文", value: "zh-CN" },
];
const handleChange = (data: unknown) => {
  global.locale = (data as { value: string }).value;
};

watch(
  () => i18n.locale.value,
  () => {
    const langPresent = options.find(o => o.value === i18n.locale.value)?.content ?? i18n.t("sel-locale.unknown");
    MessagePlugin.success(i18n.t("sel-locale.msg", { lang: langPresent }));
  },
);
</script>

<template>
  <t-dropdown :options="options" @click="handleChange">
    <t-button theme="default" variant="outline" shape="circle">
      <EarthIcon />
    </t-button>
  </t-dropdown>
</template>

<i18n lang="yaml" locale="en">
lang:
  en: English
  zh-CN: 简体中文
sel-locale:
  unknown: Unknown Language
  msg: Selected {lang}
</i18n>

<i18n lang="yaml" locale="zh-CN">
lang:
  en: English
  zh-CN: 简体中文
sel-locale:
  unknown: 未知语言
  msg: 切换至{lang}
</i18n>
