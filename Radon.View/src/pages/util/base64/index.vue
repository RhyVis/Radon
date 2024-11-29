<script setup lang="ts">
import { get } from "@vueuse/core";
import { Base64 } from "js-base64";
import { computed, ref } from "vue";
import { useI18n } from "vue-i18n";

const { t } = useI18n();

enum Mode {
  Encode = "encode",
  Decode = "decode",
}

const input = ref("");
const inputState = computed(() => {
  if (mode.value == Mode.Decode) {
    if (Base64.isValid(input.value)) {
      return "success";
    } else {
      return "error";
    }
  } else {
    return "default";
  }
});
const output = computed(() => {
  try {
    switch (get(mode)) {
      case Mode.Encode:
        return Base64.encode(input.value);
      case Mode.Decode: {
        if (Base64.isValid(input.value)) {
          return Base64.decode(input.value);
        } else {
          return t("form.invalid");
        }
      }
      default:
        return input.value;
    }
  } catch (e) {
    console.error(e);
    return "×";
  }
});
const mode = ref(Mode.Encode);
</script>

<template>
  <content-layout title="BASE64" :subtitle="t('st')">
    <t-form>
      <t-form-item :label="t('form.input')">
        <t-textarea v-model="input" :status="inputState" :autosize="true" />
      </t-form-item>
      <t-form-item :label="t('form.mode.label')">
        <t-radio-group v-model="mode">
          <t-radio-button :label="t('form.mode.encode')" value="encode" />
          <t-radio-button :label="t('form.mode.decode')" value="decode" />
        </t-radio-group>
      </t-form-item>
      <t-form-item :label="t('form.output')">
        <t-textarea :value="output" :autosize="true" />
      </t-form-item>
      <t-form-item :label="t('form.tool')">
        <t-space :size="5">
          <btn-copy :value="output" />
          <btn-read v-model="input" />
          <btn-clear v-model="input" />
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
  invalid: "Invalid Base64 string"
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
  invalid: "无效的 Base64 字符串"
</i18n>
