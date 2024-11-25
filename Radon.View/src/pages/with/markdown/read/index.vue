<script setup lang="ts">
import { getMdRecord } from "@/pages/with/markdown/define.ts";
import { get, set, useDark, useWindowSize } from "@vueuse/core";
import { useRouteParams } from "@vueuse/router";
import { MdCatalog, MdPreview } from "md-editor-v3";
import "md-editor-v3/lib/preview.css";
import { ArrowLeftIcon } from "tdesign-icons-vue-next";
import { Content as TContent } from "tdesign-vue-next";
import { computed, ref, watch } from "vue";
import { useRouter } from "vue-router";

const router = useRouter();
const dark = useDark();
const path = useRouteParams("p");
const content = ref("");
const scrollElement = document.documentElement;

const { width } = useWindowSize();
const name = ref("");
const sideWidth = computed(() => (get(width) > 768 ? "232px" : "0"));
const theme = computed(() => (get(dark) ? "dark" : "light"));

const updateContent = async (path: string) => {
  const record = await getMdRecord(path);
  set(name, record.name);
  set(content, record.content);
};

watch(
  () => path.value,
  async newPath => {
    if (newPath) {
      if (typeof newPath === "string") {
        await updateContent(newPath);
      } else {
        await updateContent(newPath[0]);
      }
    }
  },
  { immediate: true },
);
</script>

<template>
  <content-layout title="Reader" :subtitle="name">
    <t-layout>
      <t-content>
        <MdPreview id="preview-only" :model-value="content" :theme="theme" />
      </t-content>
      <t-aside :width="sideWidth">
        <MdCatalog editor-id="preview-only" :scroll-element="scrollElement" />
      </t-aside>
    </t-layout>

    <template #actions>
      <t-button class="r-no-select" variant="text" theme="primary" shape="circle" @click="router.push('/md')">
        <ArrowLeftIcon />
      </t-button>
    </template>
  </content-layout>
</template>
