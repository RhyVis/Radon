<script setup lang="ts">
import { getMdIndex, type MdIndex } from "@/pages/with/markdown/scripts/define.ts";
import { set, useToggle } from "@vueuse/core";
import { EditIcon, SearchIcon } from "tdesign-icons-vue-next";
import { MessagePlugin } from "tdesign-vue-next";
import { onMounted, ref } from "vue";
import { useRouter } from "vue-router";

const router = useRouter();
const indexList = ref<MdIndex[]>([]);
const [loading, setLoading] = useToggle(true);

const handleToRead = (path: string) => router.push(`md/${path}/read`);
const handleToWrite = (path: string) => router.push(`md/${path}/write`);

onMounted(() => {
  getMdIndex()
    .then(i => set(indexList, i))
    .catch(() => MessagePlugin.error("获取数据失败"))
    .finally(() => setLoading(false));
});
</script>

<template>
  <content-layout title="档案馆">
    <t-text class="r-no-select" v-if="loading" style="text-align: center" :strong="true">Loading...</t-text>
    <t-list v-else :split="true">
      <t-list-item v-for="(entry, index) in indexList" :key="index">
        <t-list-item-meta :title="entry.name" :description="entry.desc" />
        <template #action>
          <t-space>
            <t-button theme="primary" variant="outline" shape="circle" @click="handleToRead(entry.path)">
              <SearchIcon />
            </t-button>
            <t-button theme="primary" variant="outline" shape="circle" @click="handleToWrite(entry.path)">
              <EditIcon />
            </t-button>
          </t-space>
        </template>
      </t-list-item>
    </t-list>
  </content-layout>
</template>

<style scoped></style>
