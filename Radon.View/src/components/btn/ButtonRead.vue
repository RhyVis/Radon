<script setup lang="ts">
import { BrushIcon } from "tdesign-icons-vue-next";
import { MessagePlugin } from "tdesign-vue-next";

defineProps({
  target: {
    type: String,
    required: true,
  },
});

const { t } = useI18n();
const emit = defineEmits(["update:target"]);
const action = async () => {
  try {
    const text = await navigator.clipboard.readText();
    emit("update:target", text);
    await MessagePlugin.success(t("readClipboardSuccess"));
  } catch (e) {
    console.error("Fail in reading clipboard content: ", e);
    await MessagePlugin.error(t("readClipboardFail"));
  }
};
</script>

<template>
  <t-tooltip :content="t('readClipboard')" placement="top">
    <t-button theme="default" shape="square" @click="action">
      <BrushIcon />
    </t-button>
  </t-tooltip>
</template>

<i18n lang="yaml" locale="en">
readClipboard: Read Clipboard
readClipboardSuccess: Read Clipboard Success
readClipboardFail: Read Clipboard Fail
</i18n>

<i18n lang="yaml" locale="zh-CN">
readClipboard: 读取剪贴板
readClipboardSuccess: 读取剪贴板成功
readClipboardFail: 读取剪贴板失败
</i18n>
