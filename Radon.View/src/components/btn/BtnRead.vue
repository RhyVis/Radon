<script lang="ts" setup>
import { BrushIcon } from "tdesign-icons-vue-next";
import { MessagePlugin } from "tdesign-vue-next";
import { useI18n } from "vue-i18n";

const emit = defineEmits<{
  success: [];
  error: [];
}>();
const model = defineModel<string>({ required: true });
const { t } = useI18n();
const action = () => {
  try {
    navigator.clipboard.readText().then(val => (model.value = val));
    MessagePlugin.success(t("msg.success"));
    emit("success");
  } catch (e) {
    console.error("Fail in reading clipboard content: ", e);
    MessagePlugin.error(t("msg.failure"));
    emit("error");
  }
};
</script>

<template>
  <t-tooltip :content="t('action')" placement="top">
    <t-button shape="square" theme="default" @click="action">
      <BrushIcon />
    </t-button>
  </t-tooltip>
</template>

<i18n lang="yaml" locale="en">
action: Read Clipboard
msg:
  success: Read Clipboard Success
  failure: Read Clipboard Fail
</i18n>

<i18n lang="yaml" locale="zh-CN">
action: 读取剪贴板
msg:
  success: 读取剪贴板成功
  failure: 读取剪贴板失败
</i18n>
