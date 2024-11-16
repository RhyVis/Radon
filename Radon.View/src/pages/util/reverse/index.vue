<script setup lang="ts">
import { reverseModes } from "@/pages/util/reverse/scripts/reverse";
import { RefreshIcon } from "tdesign-icons-vue-next";
import { MessagePlugin } from "tdesign-vue-next";
import { reactive, ref } from "vue";

const query = reactive({
  text: "",
  mode: "reverse",
});

const handleReverse = async () => {
  if (query.text.length > 0) {
    const reversed = [...query.text].reverse().join("");
    switch (query.mode) {
      case "reverse":
        result.value = reversed;
        break;
      case "fold-in":
        result.value = query.text + reversed;
        break;
      case "fold-out":
        result.value = reversed + query.text;
        break;
      default:
        console.log(`Unexpected mode: ${query.mode}`);
        break;
    }
  } else {
    result.value = "你很喜欢这样吗";
    await MessagePlugin.warning("空着干什么...");
  }
};

const result = ref("");
</script>

<template>
  <content-layout title="倒叙者" subtitle="别急有反转">
    <t-form>
      <t-form-item label="原始文本">
        <t-textarea v-model="query.text" placeholder="空白是你的谎言" />
      </t-form-item>
      <t-form-item label="模式">
        <t-radio-group v-model="query.mode" size="small">
          <t-radio-button v-for="(mode, index) in reverseModes" :value="mode.value" :key="index" :label="mode.label" />
        </t-radio-group>
      </t-form-item>
      <t-form-item label="结果">
        <t-textarea :value="result" :readonly="true" :autosize="true" />
      </t-form-item>
      <t-form-item label="执行">
        <t-button shape="round" @click="handleReverse">
          <RefreshIcon />
        </t-button>
      </t-form-item>
      <t-form-item label="工具">
        <t-space :size="5">
          <btn-copy :target="result" />
          <btn-read v-model="query.text" />
          <btn-clear v-model="query.text" />
        </t-space>
      </t-form-item>
    </t-form>
  </content-layout>
</template>
