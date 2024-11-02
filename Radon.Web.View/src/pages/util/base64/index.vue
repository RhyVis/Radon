<script setup lang="ts">
import ButtonClear from "@/components/btn/ButtonClear.vue";
import ButtonCopy from "@/components/btn/ButtonCopy.vue";
import ButtonRead from "@/components/btn/ButtonRead.vue";
import ContentLayout from "@/layout/frame/ContentLayout.vue";
import { Base64 } from "js-base64";
import { computed, ref } from "vue";

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
  <ContentLayout title="BASE64" subtitle="编码、校验与解码">
    <t-form>
      <t-form-item label="输入">
        <t-textarea v-model="inputRef" :status="inputStatusRef" :autosize="true" />
      </t-form-item>
      <t-form-item label="模式">
        <t-radio-group v-model="modeRef">
          <t-radio-button label="编码" value="encode" />
          <t-radio-button label="解码" value="decode" />
        </t-radio-group>
      </t-form-item>
      <t-form-item label="输出">
        <t-textarea :value="outputRef" :autosize="true" />
      </t-form-item>
      <t-form-item label="工具">
        <t-space :size="5">
          <ButtonCopy :target="outputRef" />
          <ButtonRead v-model:target="inputRef" />
          <ButtonClear v-model:target="inputRef" />
        </t-space>
      </t-form-item>
    </t-form>
  </ContentLayout>
</template>
