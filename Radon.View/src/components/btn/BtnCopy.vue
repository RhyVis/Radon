<script lang="ts" setup>
import { useClipboard } from "@vueuse/core";
import { CopyIcon } from "tdesign-icons-vue-next";
import { MessagePlugin } from "tdesign-vue-next";
import { useI18n } from "vue-i18n";

const emit = defineEmits<{
  success: [];
  error: [];
}>();
const { value = "" } = defineProps<{ value: string }>();
const { t } = useI18n();
const { copy } = useClipboard();

const action = async () => {
  if (value.length > 0) {
    try {
      await copy(value);
      await MessagePlugin.success(t("msg.success"));
      emit("success");
    } catch (e) {
      console.error(e);
      await MessagePlugin.error(t("msg.failure"));
      emit("error");
    }
  } else {
    await MessagePlugin.error(t("msg.emptyContent"));
    emit("error");
  }
};
</script>

<template>
  <t-tooltip :content="t('action')" placement="top">
    <t-button shape="square" theme="default" @click="action">
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
