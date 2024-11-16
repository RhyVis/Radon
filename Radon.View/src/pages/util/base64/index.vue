<script setup lang="ts">
import { Base64 } from "js-base64";
import { computed, ref } from "vue";
import { useI18n } from "vue-i18n";

const { t } = useI18n();
const inputRef = ref("");
const inputStatusRef = computed(() => {
  if (modeRef.value == "decode") {
    if (Base64.isValid(inputRef.value)) {
      return "success";
    } else {
      return "error";
    }
  } else {
    return "default";
  }
});
const outputRef = computed(() => {
  try {
    switch (modeRef.value) {
      case "encode":
        return Base64.encode(inputRef.value);
      case "decode":
        return Base64.decode(inputRef.value);
      default:
        return inputRef.value;
    }
  } catch (e) {
    console.error(e);
    return "Exception";
  }
});
const modeRef = ref("encode");
</script>

<template>
  <content-layout title="BASE64" :subtitle="t('st')">
    <t-form>
      <t-form-item :label="t('form.input')">
        <t-textarea v-model="inputRef" :status="inputStatusRef" :autosize="true" />
      </t-form-item>
      <t-form-item :label="t('form.mode.label')">
        <t-radio-group v-model="modeRef">
          <t-radio-button :label="t('form.mode.encode')" value="encode" />
          <t-radio-button :label="t('form.mode.decode')" value="decode" />
        </t-radio-group>
      </t-form-item>
      <t-form-item :label="t('form.output')">
        <t-textarea :value="outputRef" :autosize="true" />
      </t-form-item>
      <t-form-item :label="t('form.tool')">
        <t-space :size="5">
          <btn-copy :target="outputRef" />
          <btn-read v-model="inputRef" />
          <btn-clear v-model="inputRef" />
        </t-space>
      </t-form-item>
    </t-form>
  </content-layout>
</template>

<i18n locale="en">
st: "Encode, Verify and Decode"
form:
  input: "Input"
  mode:
    label: "Mode"
    encode: "Encode"
    decode: "Decode"
  output: "Output"
  tool: "Tool"
</i18n>

<i18n locale="zh-CN">
st: "编码、校验与解码"
form:
  input: "输入"
  mode:
    label: "模式"
    encode: "编码"
    decode: "解码"
  output: "输出"
  tool: "工具"
</i18n>
