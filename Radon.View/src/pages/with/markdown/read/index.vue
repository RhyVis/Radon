<script setup lang="ts">
import { getMdRecord } from "@/pages/with/markdown/scripts/define.ts";
import { set } from "@vueuse/core";
import { useRouteParams } from "@vueuse/router";
import { MdCatalog, MdPreview } from "md-editor-v3";
import "md-editor-v3/lib/preview.css";
import { ArrowLeftIcon } from "tdesign-icons-vue-next";
import { ref, watch } from "vue";
import { useRouter } from "vue-router";

const router = useRouter();
const path = useRouteParams("path");
const content = ref<string>("");
const scrollElement = document.documentElement;

const updateContent = async (path: string) => set(content, (await getMdRecord(path)).content);

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
  <content-layout title="Reader">
    <MdPreview id="preview-only" :model-value="content" />
    <MdCatalog editor-id="preview-only" :scroll-element="scrollElement" />

    <template #actions>
      <t-button class="r-no-select" variant="text" theme="primary" shape="circle" @click="router.push('/md')">
        <ArrowLeftIcon />
      </t-button>
    </template>
  </content-layout>
</template>
