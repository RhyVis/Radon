<script setup lang="ts">
import { createMdRecord } from "@/pages/with/markdown/define.ts";
import { get } from "@vueuse/core";
import { useRouteQuery } from "@vueuse/router";
import { MdEditor } from "md-editor-v3";
import "md-editor-v3/lib/style.css";
import { ArrowLeftIcon } from "tdesign-icons-vue-next";
import { MessagePlugin } from "tdesign-vue-next";
import { onMounted, ref } from "vue";
import { useRouter } from "vue-router";

const router = useRouter();
const createName = useRouteQuery("name");
const createDesc = useRouteQuery("desc");
const content = ref("");

const handleCreate = async () => {
  const path = await createMdRecord(get(createName) as string, get(createDesc) as string, get(content));
  if (path.length > 0) {
    void MessagePlugin.success("Create success, moving to write page");
    await router.push(`/md/${path}/write`);
  } else {
    void MessagePlugin.error("Create failed");
  }
};

onMounted(() => {
  if (
    createName.value == null ||
    createDesc.value == null ||
    typeof createName.value !== "string" ||
    typeof createDesc.value !== "string"
  ) {
    MessagePlugin.warning("Invalid create request");
    router.push("/md");
  }
});
</script>

<template>
  <content-layout title="Create">
    <MdEditor v-model="content" @onSave="handleCreate()" noUploadImg />

    <template #actions>
      <t-button class="r-no-select" variant="text" theme="primary" shape="circle">
        <ArrowLeftIcon />
      </t-button>
    </template>
  </content-layout>
</template>
