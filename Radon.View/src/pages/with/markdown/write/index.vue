<script setup lang="ts">
import { checkMdRecord, getMdRecord, type MdIndex, updateMdRecord } from "@/pages/with/markdown/define.ts";
import { get, set } from "@vueuse/core";
import { useRouteParams } from "@vueuse/router";
import { MdEditor } from "md-editor-v3";
import "md-editor-v3/lib/style.css";
import { ArrowLeftIcon } from "tdesign-icons-vue-next";
import { MessagePlugin } from "tdesign-vue-next";
import { onMounted, ref, watch } from "vue";
import { useRouter } from "vue-router";

const router = useRouter();
const path = useRouteParams("p");
const content = ref("");
const record = ref<MdIndex>({
  path: "",
  name: "",
  desc: "",
});

const updateContent = async (path: string) => set(content, (await getMdRecord(path)).content);
const handleUpdate = () =>
  updateMdRecord(path.value as string, record.value.name, record.value.desc, content.value)
    .then(() => MessagePlugin.success("Update success"))
    .catch(() => MessagePlugin.error("Update failed"));

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

onMounted(() => {
  checkMdRecord(get(path) as string)
    .then(res => set(record, res))
    .then(() => {
      if (record.value.name.length === 0) {
        MessagePlugin.error("No such record");
        router.push("/md");
      }
    });
});
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
