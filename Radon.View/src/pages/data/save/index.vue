<script lang="ts" setup>
import type { SaveEntry } from "@/pages/data/save/scripts/define.ts";
import {
  textStoreDelete,
  textStoreQuery,
  textStoreQueryAll,
  textStoreUpdate,
} from "@/pages/data/save/scripts/function.ts";
import { useSaveStore } from "@/pages/data/save/scripts/store";
import { get, set } from "@vueuse/core";
import { storeToRefs } from "pinia";
import { DeleteIcon, DownloadIcon, FileDownloadIcon, UploadIcon } from "tdesign-icons-vue-next";
import { MessagePlugin, type TableProps, Tag } from "tdesign-vue-next";
import { ref, shallowRef } from "vue";
import { useI18n } from "vue-i18n";

const store = useSaveStore();
const { t } = useI18n();
const { loading, id, qText, qNote, rText, rNote } = storeToRefs(store);
const all = shallowRef<SaveEntry[]>([]);
const allColumns = ref<TableProps["columns"]>([
  {
    colKey: "id",
    title: "ID",
    width: 75,
    cell: (h, props) => h(Tag, { type: "primary", class: ["r-no-select"] }, props.row.id),
  },
  {
    colKey: "note",
    title: t("col.note"),
  },
]);
const optEl = document.getElementById("text-store-opt-area");

const handleStore = async () => {
  if (get(qText).length === 0) {
    await MessagePlugin.warning(t("msg.empty"));
    return;
  }

  set(loading, true);
  void MessagePlugin.warning(t("msg.warning"));

  try {
    const b = await textStoreUpdate(store.query);
    if (b) {
      await MessagePlugin.success(t("msg.store"));
    } else {
      await MessagePlugin.error(t("msg.storeFail"));
    }
  } catch (e) {
    console.error(e);
    await MessagePlugin.error(t("msg.storeFail"));
  } finally {
    set(loading, false);
  }
};
const handleQuery = async () => {
  set(loading, true);
  void MessagePlugin.warning(t("msg.warning"));
  try {
    const { data, code } = await textStoreQuery(get(id));

    if (code < 0) {
      void MessagePlugin.warning(t("msg.selectNothing"));
      set(loading, false);
      return;
    }

    store.$patch({
      rText: data.text,
      rNote: data.note,
      qText: data.text,
      qNote: data.note,
    });

    void MessagePlugin.success(t("msg.select"));
  } catch (e) {
    console.error(e);
    void MessagePlugin.error(t("msg.selectFail"));
  } finally {
    set(loading, false);
  }
};
const handleQueryAll = async () => {
  set(loading, true);
  void MessagePlugin.warning(t("msg.warning"));
  try {
    const { data, code } = await textStoreQueryAll();

    if (code < 0) {
      void MessagePlugin.warning(t("msg.selectNothing"));
      set(loading, false);
      return;
    }

    data.sort((a, b) => a.id - b.id);
    set(all, data);

    void MessagePlugin.success(t("msg.select"));
  } catch (e) {
    console.error(e);
    void MessagePlugin.error(t("msg.selectFail"));
  } finally {
    set(loading, false);
  }
};
const handleDelete = async () => {
  set(loading, true);
  void MessagePlugin.warning(t("msg.warning"));
  try {
    const b = await textStoreDelete(get(id));
    if (b) {
      await MessagePlugin.success(t("msg.delete"));
    } else {
      await MessagePlugin.error(t("msg.deleteFail"));
    }

    const { data, code } = await textStoreQueryAll();

    if (code < 0) {
      void MessagePlugin.warning(t("msg.selectNothing"));
      set(loading, false);
      return;
    }

    data.sort((a, b) => a.id - b.id);
    set(all, data);
  } catch (e) {
    console.error(e);
    await MessagePlugin.error(t("msg.deleteFail"));
  } finally {
    set(loading, false);
  }
};
const moveToActions = () => {
  if (optEl) optEl.scrollIntoView({ behavior: "smooth" });
};
</script>

<template>
  <content-layout :subtitle="t('st')" :title="t('tt')">
    <t-form>
      <t-form-item :label="t('form.store')">
        <t-textarea v-model="qText" :autosize="true" placeholder="" />
      </t-form-item>
      <t-form-item :label="t('form.note')">
        <t-input v-model="qNote" placeholder="" />
      </t-form-item>
      <t-form-item :label="t('form.read')">
        <t-textarea :autosize="true" :readonly="true" :value="rText" placeholder="" />
      </t-form-item>
      <t-form-item :label="t('form.readNote')">
        <t-input :readonly="true" :value="rNote" placeholder="" />
      </t-form-item>
      <t-divider />
      <t-form-item label="ID">
        <t-input-number v-model="id" :auto-width="true" :min="0" size="small" />
      </t-form-item>
      <t-form-item id="text-store-opt-area" :label="t('form.operation')">
        <t-loading :loading="loading" size="small">
          <t-space :size="12">
            <t-button shape="rectangle" theme="primary" @click="handleStore">
              <UploadIcon />
            </t-button>
            <t-button shape="rectangle" theme="primary" @click="handleQuery">
              <DownloadIcon />
            </t-button>
            <t-button shape="rectangle" theme="warning" @click="handleQueryAll">
              <FileDownloadIcon />
            </t-button>
            <btn-confirm v-if="all.length > 0" shape="rectangle" theme="danger" @confirm="handleDelete">
              <DeleteIcon />
            </btn-confirm>
          </t-space>
        </t-loading>
      </t-form-item>
      <t-form-item :label="t('form.tool')">
        <t-space :size="5">
          <btn-copy :value="rText" />
          <btn-read v-model="qText" @success="moveToActions" />
          <btn-clear v-model="qText" />
        </t-space>
      </t-form-item>
    </t-form>
    <div v-if="all.length > 0">
      <t-divider />
      <t-table :columns="allColumns" :data="all" :stripe="true" size="small" />
    </div>
  </content-layout>
</template>

<i18n locale="en">
tt: "Text Storage"
st: "Key-Value"
form:
  store: "Store Content"
  note: "Store Note"
  read: "Read Content"
  readNote: "Read Note"
  operation: "Operation"
  tool: "Tool"
col:
  text: "Text"
  note: "Note"
msg:
  empty: "Do not store empty"
  id: "ID cannot be less than 0"
  store: "Store Success"
  storeFail: "Store Fail"
  select: "Query Success"
  selectFail: "Query Fail"
  selectNothing: "Query Nothing"
  delete: "Delete Success"
  deleteFail: "Delete Fail"
  warning: "This may take a while, please wait patiently."
</i18n>

<i18n locale="zh-CN">
tt: "字符存储"
st: "键值对"
form:
  store: "存储内容"
  note: "存储备注"
  read: "读取内容"
  readNote: "读取备注"
  operation: "数据操作"
  tool: "工具"
col:
  text: "文本"
  note: "备注"
msg:
  empty: "不可存储空内容"
  id: "ID不可小于0"
  store: "存储成功"
  storeFail: "存储失败"
  select: "查询成功"
  selectFail: "查询失败"
  selectNothing: "查询无内容"
  delete: "删除成功"
  deleteFail: "删除失败"
  warning: "这可能需要一段时间，请耐心等待。"
</i18n>
