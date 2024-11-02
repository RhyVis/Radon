<script setup lang="ts">
import { DeleteIcon } from "tdesign-icons-vue-next";
import { MessagePlugin } from "tdesign-vue-next";

defineProps({
  target: {
    type: String,
    required: true,
  },
});

const emit = defineEmits(["update:target"]);
const action = () => {
  try {
    emit("update:target", "");
    MessagePlugin.success("清除内容成功");
  } catch (e) {
    console.error("Fail in clearing content: ", e);
    MessagePlugin.error("清除内容失败");
  }
};
</script>

<template>
  <t-tooltip content="清空输入内容" placement="top">
    <t-button theme="default" shape="square" @click="action">
      <DeleteIcon />
    </t-button>
  </t-tooltip>
</template>
