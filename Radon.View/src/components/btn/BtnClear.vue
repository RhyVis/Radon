<script lang="ts" setup>
import { DeleteIcon } from "tdesign-icons-vue-next";
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
    model.value = "";
    MessagePlugin.success(t("msg.success"));
    emit("success");
  } catch (e) {
    console.error("Fail in clearing content: ", e);
    MessagePlugin.error(t("msg.failure"));
    emit("error");
  }
};
</script>

<template>
  <t-tooltip :content="t('action')" placement="top">
    <t-button shape="square" theme="default" @click="action">
      <DeleteIcon />
    </t-button>
  </t-tooltip>
</template>

<i18n locale="en">
action: "Clear input content"
msg:
  success: "Clear content successfully"
  failure: "Fail to clear content"
</i18n>

<i18n locale="zh-CN">
action: 清空输入内容
msg:
  success: 清除内容成功
  failure: 清除内容失败
</i18n>
