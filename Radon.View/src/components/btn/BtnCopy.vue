<script setup lang="ts">
import { useClipboard } from "@vueuse/core";
import { CopyIcon } from "tdesign-icons-vue-next";
import { MessagePlugin } from "tdesign-vue-next";
import { useI18n } from "vue-i18n";

const { target = "" } = defineProps<{ target: string }>();
const { t } = useI18n();
const { copy } = useClipboard();

const action = async () => {
  if (target.length > 0) {
    try {
      await copy(target);
      await MessagePlugin.success(t("msg.success"));
    } catch (e) {
      console.error(e);
      await MessagePlugin.error(t("msg.failure"));
    }
  } else {
    await MessagePlugin.error(t("msg.emptyContent"));
  }
};
</script>

<template>
  <t-tooltip :content="t('action')" placement="top">
    <t-button theme="default" shape="square" @click="action">
      <CopyIcon />
    </t-button>
  </t-tooltip>
</template>

<i18n locale="en">
action: "Copy content"
msg:
  success: "Copy success"
  failure: "Copy fail"
  emptyContent: Can't copy empty content"
</i18n>

<i18n locale="zh-CN">
action: "复制内容"
msg: 
  success: "复制成功"
  failure: "复制失败"
  emptyContent: "不能复制空内容"
</i18n>
