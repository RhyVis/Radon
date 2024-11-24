<script setup lang="ts">
import { getMdRecord } from "@/pages/with/markdown/scripts/define.ts";
import { get, set } from "@vueuse/core";
import { useRouteParams, useRouteQuery } from "@vueuse/router";
import { MdEditor } from "md-editor-v3";
import "md-editor-v3/lib/style.css";
import { ArrowLeftIcon } from "tdesign-icons-vue-next";
import { MessagePlugin } from "tdesign-vue-next";
import { ref, watch } from "vue";
import { useRouter } from "vue-router";

const router = useRouter();
const path = useRouteParams("path");
const newContent = useRouteQuery("new", 0);
const content = ref("");

const updateContent = async (path: string) => set(content, (await getMdRecord(path)).content);
const handleUpdate = () => {
  console.log(get(newContent));
  MessagePlugin.warning("Not implemented yet");
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
  <content-layout title="Write">
    <MdEditor v-model="content" @onSave="handleUpdate()" noUploadImg />

    <template #actions>
      <t-button class="r-no-select" variant="text" theme="primary" shape="circle" @click="router.push('/md')">
        <ArrowLeftIcon />
      </t-button>
    </template>
  </content-layout>
</template>
