<script setup lang="ts">
import { SendIcon } from "tdesign-icons-vue-next";
import { MessagePlugin } from "tdesign-vue-next";

const query = reactive({
  text: "哦牛",
  type: "nmsl",
  decode: false,
});
const loading = ref(false);

const handleSpam = async () => {
  if (query.text.length === 0) {
    await MessagePlugin.warning("不要什么都不输入");
  } else {
    loading.value = true;
    try {
      result.value = (
        await apiPostStr("/api/escape", {
          text: query.text,
          type: query.type,
          encode: !query.decode,
        })
      ).data;
    } catch (e) {
      console.error(e);
      await MessagePlugin.error("与服务器通信失败");
    } finally {
      loading.value = false;
    }
  }
};

const result = ref("");
</script>

<template>
  <content-layout title="抽象翻译器" subtitle="玩抽象的这辈子有了">
    <t-form>
      <t-tabs v-model:value="query.type" @change="query.decode = false">
        <!-- 玩抽象的这辈子有了 -->
        <t-tab-panel class="mt-2" label="抽象转换" value="nmsl">
          <t-form-item label="原始文本">
            <t-textarea v-model="query.text" auto-size />
          </t-form-item>
          <t-form-item label="解密" tooltip="解析方法不全，不能完全解析Emoji">
            <t-switch v-model="query.decode" />
          </t-form-item>
        </t-tab-panel>
        <!-- 繁体 -->
        <t-tab-panel class="mt-2" label="繁体转换" value="trad">
          <t-form-item label="原始文本">
            <t-textarea v-model="query.text" auto-size />
          </t-form-item>
          <t-form-item label="解密">
            <t-switch v-model="query.decode" />
          </t-form-item>
        </t-tab-panel>
        <!-- 火星文 -->
        <t-tab-panel class="mt-2" label="火星文化" value="sprk">
          <t-form-item label="原始文本">
            <t-textarea v-model="query.text" auto-size />
          </t-form-item>
          <t-form-item label="解密">
            <t-switch v-model="query.decode" />
          </t-form-item>
        </t-tab-panel>
        <!-- 形近字 -->
        <t-tab-panel class="mt-2" label="形近转换" value="diff">
          <t-form-item label="原始文本">
            <t-textarea v-model="query.text" auto-size />
          </t-form-item>
        </t-tab-panel>
      </t-tabs>
      <div class="mt-4">
        <t-form-item label="输出结果">
          <t-loading class="r-cd-out-loading" size="small" :loading="loading" :delay="400">
            <t-textarea :model-value="result" placeholder="返回内容" :autosize="true" :readonly="true" />
          </t-loading>
        </t-form-item>
        <t-form-item label="操作">
          <t-button shape="round" :disabled="loading" @click="handleSpam">
            <SendIcon />
          </t-button>
        </t-form-item>
        <t-form-item label="工具">
          <t-space :size="5">
            <btn-copy :target="result" />
            <btn-read v-model="query.text" />
            <btn-clear v-model="query.text" />
          </t-space>
        </t-form-item>
      </div>
    </t-form>
  </content-layout>
</template>

<style scoped>
.r-cd-out-loading {
  width: 100%;
}
</style>
