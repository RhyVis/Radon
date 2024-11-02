<script setup lang="ts">
import { BrushIcon } from "tdesign-icons-vue-next";
import { MessagePlugin } from "tdesign-vue-next";

defineProps({
  target: {
    type: String,
    required: true,
  },
});

const emit = defineEmits(["update:target"]);
const action = async () => {
  try {
    const text = await navigator.clipboard.readText();
    emit("update:target", text);
    await MessagePlugin.success("读取剪贴板成功");
  } catch (e) {
    console.error("Fail in reading clipboard content: ", e);
    await MessagePlugin.error("读取剪贴板失败");
  }
};
</script>

<template>
  <t-tooltip content="读取剪贴板内容" placement="top">
    <t-button theme="default" shape="square" @click="action">
      <BrushIcon />
    </t-button>
  </t-tooltip>
</template>
