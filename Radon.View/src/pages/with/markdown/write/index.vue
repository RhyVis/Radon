<script setup lang="ts">
import { checkMdRecord, updateMdRecord } from "@/pages/with/markdown/define.ts";
import { get, useDark } from "@vueuse/core";
import { useRouteParams } from "@vueuse/router";
import { MdEditor } from "md-editor-v3";
import "md-editor-v3/lib/style.css";
import { ArrowLeftIcon } from "tdesign-icons-vue-next";
import { MessagePlugin } from "tdesign-vue-next";
import { computed, reactive, watch } from "vue";
import { useRouter } from "vue-router";

const router = useRouter();
const dark = useDark();
const path = useRouteParams("p");
const record = reactive({
  name: "",
  desc: "",
  content: "",
});
const theme = computed(() => (get(dark) ? "dark" : "light"));

const updateContent = async (path: string) => {
  const check = await checkMdRecord(path);
  if (check.name.length === 0) {
    void MessagePlugin.error("No such record");
    await router.push("/md");
  } else {
    record.name = check.name;
    record.desc = check.desc;
    record.content = check.content;
  }
};
const handleUpdate = () =>
  updateMdRecord(path.value as string, record.name, record.desc, record.content)
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
</script>

<template>
  <content-layout title="Write" :subtitle="record.name">
    <div>
      <t-empty v-if="record.content.length === 0" />
      <MdEditor v-else v-model="record.content" @onSave="handleUpdate()" :theme="theme" noUploadImg />
    </div>

    <template #actions>
      <t-button class="r-no-select" variant="text" theme="primary" shape="circle" @click="router.push('/md')">
        <ArrowLeftIcon />
      </t-button>
    </template>
  </content-layout>
</template>
