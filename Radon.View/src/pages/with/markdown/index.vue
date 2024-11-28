<script setup lang="ts">
import { useNarrow } from "@/composable/useNarrow.ts";
import { deleteMdRecord, getMdIndex, type MdIndexDto } from "@/pages/with/markdown/define.ts";
import { useMdStore } from "@/pages/with/markdown/store.ts";
import { useGlobalStore } from "@/store/global.ts";
import { get, set, useToggle } from "@vueuse/core";
import { storeToRefs } from "pinia";
import { AddCircleIcon, DeleteIcon, EditIcon, HomeIcon, SearchIcon } from "tdesign-icons-vue-next";
import { MessagePlugin } from "tdesign-vue-next";
import { computed, onMounted, reactive, ref } from "vue";
import { useI18n } from "vue-i18n";
import { useRouter } from "vue-router";

const router = useRouter();
const store = useMdStore();
const { t } = useI18n();
const { authPassed } = storeToRefs(useGlobalStore());
const { indexList } = storeToRefs(store);
const [loading, setLoading] = useToggle(true);
const [create, setCreate] = useToggle(false);
const narrow = useNarrow();
const createRequest = reactive({ name: "", desc: "" });
const popupVisible = ref(false);
const delBtnTheme = computed(() => (popupVisible.value ? "danger" : "primary"));

const date = (entry: MdIndexDto) =>
  get(narrow)
    ? new Date(entry.updateTime).toLocaleDateString()
    : `${new Date(entry.updateTime).toLocaleDateString()} Since ${new Date(entry.createTime).toLocaleDateString()}`;
const handleCreate = () => {
  if (createRequest.name.length === 0 || createRequest.desc.length === 0) {
    MessagePlugin.warning(t("msg.nameDescRequired"));
    return;
  }
  router.push({ path: "/archive/create", query: createRequest });
};
const handleDelete = (path: string) =>
  deleteMdRecord(path)
    .then(() => MessagePlugin.success(t("msg.delSuccess")))
    .catch(() => MessagePlugin.warning(t("msg.delFailed")))
    .then(updateIndex);
const handleToRead = (path: string) => router.push(`/archive/${path}/read`);
const handleToWrite = (path: string) => router.push(`/archive/${path}/edit`);
const updateIndex = () =>
  getMdIndex()
    .then(i => set(indexList, i))
    .catch(() => MessagePlugin.error(t("msg.updateIndexFailed")))
    .finally(() => setLoading(false));

onMounted(() => (indexList.value.length === 0 ? updateIndex() : setLoading(false)));
</script>

<template>
  <content-layout :title="t('tt')" :subtitle="t('st')">
    <div class="mt-6" v-if="loading">
      <t-empty :title="t('common.loading')" />
    </div>
    <div class="mt-6" v-else-if="indexList.length === 0">
      <t-empty :title="t('common.noContent')" />
    </div>
    <t-card v-else>
      <t-list :stripe="true" size="small">
        <t-list-item v-for="(entry, index) in indexList" :key="index">
          <t-list-item-meta :title="entry.name" :description="entry.desc" />
          <span class="r-entry-time-display">{{ date(entry) }}</span>
          <template #action>
            <t-space :size="6">
              <t-button theme="primary" variant="text" size="small" shape="circle" @click="handleToRead(entry.path)">
                <SearchIcon />
              </t-button>
              <template v-if="authPassed">
                <t-button theme="primary" variant="text" size="small" shape="circle" @click="handleToWrite(entry.path)">
                  <EditIcon />
                </t-button>
                <t-popconfirm
                  v-model:visible="popupVisible"
                  theme="warning"
                  @confirm="handleDelete(entry.path)"
                  :content="t('popup.content')"
                  :cancel-btn="{
                    content: t('popup.cancel'),
                    theme: 'default',
                  }"
                  :confirm-btn="{
                    content: t('popup.confirm'),
                    theme: 'danger',
                  }"
                >
                  <t-button :theme="delBtnTheme" variant="text" size="small" shape="circle">
                    <DeleteIcon />
                  </t-button>
                </t-popconfirm>
              </template>
            </t-space>
          </template>
        </t-list-item>
      </t-list>
    </t-card>
    <t-form v-if="create" @submit="handleCreate">
      <t-divider />
      <t-form-item :label="t('create.name')">
        <t-input v-model="createRequest.name" />
      </t-form-item>
      <t-form-item :label="t('create.desc')">
        <t-input v-model="createRequest.desc" />
      </t-form-item>
      <t-form-item>
        <t-button type="submit">
          {{ t("create.submit") }}
        </t-button>
      </t-form-item>
    </t-form>
    <template #actions>
      <t-tooltip v-if="authPassed" placement="bottom" :content="t('create.tt')">
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

<style scoped>
.r-entry-time-display {
  font-size: smaller;
  font-style: italic;
}
</style>

<i18n locale="en">
tt: "Archive"
st: "A memo tool written when I'm free"
msg:
  nameDescRequired: "Name and Desc are required"
  delSuccess: "Successfully deleted"
  delFailed: "Failed to delete"
  updateIndexFailed: "Failed to update index"
popup:
  confirm: "Yes"
  cancel: "No"
  content: "Will you really delete it?"
create:
  tt: "Create New Entry"
  name: "Name"
  desc: "Desc"
  submit: "Create"
</i18n>

<i18n locale="zh-CN">
tt: "归档"
st: "闲的没事写的备忘录工具"
msg:
  nameDescRequired: "名称和描述不能为空"
  delSuccess: "删除成功"
  delFailed: "删除失败"
  updateIndexFailed: "更新索引失败"
popup:
  confirm: "是"
  cancel: "否"
  content: "真的要删除吗？"
create:
  tt: "新建条目"
  name: "名称"
  desc: "描述"
  submit: "创建"
</i18n>
