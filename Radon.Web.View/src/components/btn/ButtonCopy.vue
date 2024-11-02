<script setup lang="ts">
import { CopyIcon } from "tdesign-icons-vue-next";
import { MessagePlugin } from "tdesign-vue-next";
import useClipboard from "vue-clipboard3";

const props = defineProps({
  target: {
    type: String,
    default: "",
    required: true,
  },
});

const { toClipboard } = useClipboard();

const action = async () => {
  const { target } = props;
  if (target.length > 0) {
    try {
      await toClipboard(target);
      await MessagePlugin.success("复制成功");
    } catch (e) {
      console.error(e);
      await MessagePlugin.error("复制失败");
    }
  } else {
    await MessagePlugin.error("不能复制空内容");
  }
};
</script>

<template>
  <t-tooltip content="复制输出内容" placement="top">
    <t-button theme="default" shape="square" @click="action">
      <CopyIcon />
    </t-button>
  </t-tooltip>
</template>
