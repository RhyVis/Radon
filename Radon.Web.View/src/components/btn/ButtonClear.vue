<script setup lang="ts">
import { DeleteIcon } from "tdesign-icons-vue-next";
import { MessagePlugin } from "tdesign-vue-next";

defineProps({
  target: {
    type: String,
    required: true,
  },
});

const { t } = useI18n();
const emit = defineEmits(["update:target"]);
const action = () => {
  try {
    emit("update:target", "");
    MessagePlugin.success(t("btnCopy.clearSuccess"));
  } catch (e) {
    console.error("Fail in clearing content: ", e);
    MessagePlugin.error(t("btnCopy.clearFail"));
  }
};
</script>

<template>
  <t-tooltip :content="t('btnCopy.clear')" placement="top">
    <t-button theme="default" shape="square" @click="action">
      <DeleteIcon />
    </t-button>
  </t-tooltip>
</template>

<i18n lang="yaml">
en:
  btnCopy:
    clear: "Clear input content"
    clearSuccess: "Clear content successfully"
    clearFail: "Fail to clear content"
zh-CN:
  btnCopy:
    clear: "清空输入内容"
    clearSuccess: "清除内容成功"
    clearFail: "清除内容失败"
</i18n>
