<script setup lang="ts">
import { useGlobalStore } from "@/store/global";
import { EarthIcon } from "tdesign-icons-vue-next";
import { MessagePlugin } from "tdesign-vue-next";
import { watch } from "vue";
import { useI18n } from "vue-i18n";

const i18n = useI18n();
const global = useGlobalStore();

type LocaleOption = { content: string; value: string };
const options: LocaleOption[] = [
  { content: "English", value: "en" },
  { content: "简体中文", value: "zh-CN" },
];
const handleChange = (data: LocaleOption) => {
  global.locale = data.value;
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

<i18n lang="yaml">
en:
  lang:
    en: English
    zh-CN: 简体中文
  sel-locale:
    unknown: Unknown Language
    msg: Selected {lang}
zh-CN:
  lang:
    en: English
    zh-CN: 简体中文
  sel-locale:
    unknown: 未知语言
    msg: 切换至{lang}
</i18n>
