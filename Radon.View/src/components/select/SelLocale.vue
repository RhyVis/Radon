<script setup lang="ts">
import { useGlobalStore } from "@/store/global";
import { get, set } from "@vueuse/core";
import { storeToRefs } from "pinia";
import { EarthIcon } from "tdesign-icons-vue-next";
import { type DropdownOption, MessagePlugin } from "tdesign-vue-next";
import { watch } from "vue";
import { useI18n } from "vue-i18n";

const i18n = useI18n();
const global = useGlobalStore();
const { locale } = storeToRefs(global);

const options: DropdownOption[] = [
  { content: "English", value: "en" },
  { content: "简体中文", value: "zh-CN" },
];
const handleChange = (data: DropdownOption) => {
  set(locale, data.value);
};

watch(
  () => i18n.locale,
  () => {
    const langPresent = options.find(o => o.value === get(i18n.locale))?.content ?? i18n.t("unknown");
    MessagePlugin.success(i18n.t("msg", { lang: langPresent }));
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
unknown: Unknown Language
msg: Selected {lang}
</i18n>

<i18n lang="yaml" locale="zh-CN">
unknown: 未知语言
msg: 切换至{lang}
</i18n>
