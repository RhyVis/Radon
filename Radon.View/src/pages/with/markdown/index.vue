<script setup lang="ts">
import { deleteMdRecord, getMdIndex, type MdIndex } from "@/pages/with/markdown/define.ts";
import { useGlobalStore } from "@/store/global.ts";
import { set, useToggle } from "@vueuse/core";
import { storeToRefs } from "pinia";
import { AddCircleIcon, DeleteIcon, EditIcon, HomeIcon, SearchIcon } from "tdesign-icons-vue-next";
import { MessagePlugin } from "tdesign-vue-next";
import { onMounted, reactive, ref } from "vue";
import { useRouter } from "vue-router";

const router = useRouter();
const { authPassed } = storeToRefs(useGlobalStore());
const [loading, setLoading] = useToggle(true);
const [create, setCreate] = useToggle(false);
const indexList = ref<MdIndex[]>([]);
const createRequest = reactive({ name: "", desc: "" });

const handleCreate = () => {
  if (createRequest.name.length === 0 || createRequest.desc.length === 0) {
    MessagePlugin.warning("Name and Desc are required");
    return;
  }
  router.push({ path: "/md/create", query: createRequest });
};
const handleDelete = (path: string) =>
  deleteMdRecord(path)
    .then(() => MessagePlugin.success("Successfully deleted"))
    .catch(() => MessagePlugin.error("Failed to delete"))
    .then(updateIndex);
const handleToRead = (path: string) => router.push(`md/${path}/read`);
const handleToWrite = (path: string) => router.push(`md/${path}/write`);
const updateIndex = () =>
  getMdIndex()
    .then(i => set(indexList, i))
    .catch(() => MessagePlugin.error("Failed to update index"))
    .finally(() => setLoading(false));

onMounted(() => {
  updateIndex();
});
</script>

<template>
  <content-layout title="档案馆" subtitle="闲的没事写的备忘录工具">
    <t-empty v-if="loading || indexList.length == 0" />
    <t-card v-else>
      <t-list :split="true" :stripe="true">
        <t-list-item v-for="(entry, index) in indexList" :key="index">
          <t-list-item-meta :title="entry.name" :description="entry.desc" />
          <template #action>
            <t-space>
              <t-button theme="primary" variant="outline" shape="circle" @click="handleToRead(entry.path)">
                <SearchIcon />
              </t-button>
              <template v-if="authPassed">
                <t-button theme="primary" variant="outline" shape="circle" @click="handleToWrite(entry.path)">
                  <EditIcon />
                </t-button>
                <t-popconfirm theme="warning" content="Will you really delete it?" @confirm="handleDelete(entry.path)">
                  <t-button theme="primary" variant="outline" shape="circle">
                    <DeleteIcon />
                  </t-button>
                </t-popconfirm>
              </template>
            </t-space>
          </template>
        </t-list-item>
      </t-list>
    </t-card>
    <t-form v-if="create">
      <t-divider />
      <t-form-item label="Name">
        <t-input v-model="createRequest.name" />
      </t-form-item>
      <t-form-item label="Desc">
        <t-input v-model="createRequest.desc" />
      </t-form-item>
      <t-form-item>
        <t-button @click="handleCreate">Create</t-button>
      </t-form-item>
    </t-form>
    <template #actions>
      <t-tooltip v-if="authPassed" placement="bottom" content="Create new entry">
        <t-button variant="text" theme="primary" shape="circle" @click="setCreate()">
          <AddCircleIcon />
        </t-button>
      </t-tooltip>
      <RouterLink to="/">
        <t-button variant="text" theme="primary" shape="circle">
          <HomeIcon />
        </t-button>
      </RouterLink>
    </template>
  </content-layout>
</template>
