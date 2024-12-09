﻿<script lang="ts" setup>
import { useNarrow } from "@/composable/useNarrow.ts";
import { apiPostWithFile } from "@/lib/common/apiMethods";
import "@/pages/util/pdx-parser/pdx-color.css";
import type { PdxLangEventItem, PdxLangItem } from "@/pages/util/pdx-parser/scripts/define";
import {
  buildPdxLangTree,
  getPdxLangParsedEventItemById,
  getPdxLangParsedItemById,
  renderPdxColor,
  replacePdxAlias,
  sepTextContent,
} from "@/pages/util/pdx-parser/scripts/function.ts";
import { usePdxStore } from "@/pages/util/pdx-parser/scripts/store";
import { useGlobalStore } from "@/store/global";
import { get, onKeyStroke, set, useDark, useToggle } from "@vueuse/core";
import { storeToRefs } from "pinia";
import {
  AddIcon,
  ArrowDownIcon,
  ArrowUpIcon,
  DeleteIcon,
  DownloadIcon,
  HomeIcon,
  InfoCircleIcon,
  ListIcon,
  Move1Icon,
  RefreshIcon,
  SettingIcon,
  Upload1Icon,
  UploadIcon,
} from "tdesign-icons-vue-next";
import {
  MessagePlugin,
  Content as TContent,
  type RequestMethodResponse,
  type SizeEnum,
  type TdStickyItemProps,
  type TreeNodeValue,
  type TreeProps,
  type UploadFile,
} from "tdesign-vue-next";
import { computed, onMounted, ref, shallowRef } from "vue";
import { useI18n } from "vue-i18n";

const global = useGlobalStore();
const store = usePdxStore();
const narrow = useNarrow();
const [eventDrawerVisible, setEventDrawerVisible] = useToggle(false);
const [treeDrawerVisible, setTreeDrawerVisible] = useToggle(false);
const [menuUploadVisible, setMenuUploadVisible] = useToggle(false);
const [menuReplaceVisible, setMenuReplaceVisible] = useToggle(false);
const [menuReplaceAddVisible, setMenuReplaceAddVisible] = useToggle(false);
const [idFetchLoading, setIdFetchLoading] = useToggle(false);
const [fileUploading, setFileUploading] = useToggle(false);
const { initialized, replacer, addReplacerKey, addReplacerValue, requestTextStorageId } = storeToRefs(store);
const { authPassed } = storeToRefs(global);
const { t } = useI18n();
const dark = useDark();
const parseOption = ref("tree");
const treeVal = ref<TreeNodeValue[]>([]);
const treeSel = computed(() => {
  const val = treeVal.value[0];
  if (val) {
    return sepTextContent(val.toString());
  } else {
    return [];
  }
});
const sideWidth = computed(() => (!get(narrow) ? "245px" : "0"));

const treeMode = computed(() => resultTree.value?.length ?? -1 > 0);
const eventMode = computed(() => eventResult.value.length > 0);

const baseLayoutEl = document.getElementById("base-content");

const eventSel = computed(() => {
  const findNonEmptyName = (id: number): string => {
    if (id < 0) return "";
    const name = eventResult.value[id]?.name || "";
    return name.length > 0 ? name : findNonEmptyName(id - 1);
  };

  const id = eventSelId.value;
  const event = eventResult.value[id];
  return {
    ...event,
    name: event?.name.length > 0 ? event.name : findNonEmptyName(id - 1),
  };
});
const eventSelId = ref(0);
const eventResult = ref<PdxLangEventItem[]>([]);
const resultTree = shallowRef<TreeProps["data"]>([]);
const stickToolProp = ref({
  placement: "bottom-right" as "left-bottom" | "right-bottom",
  offset: [-50, 20],
});

const textRender = (raw: string) => renderPdxColor(raw, get(replacer), get(dark));
const textAlias = (raw: string) => replacePdxAlias(raw, get(replacer));
const parse = (list: PdxLangItem[]) => set(resultTree, buildPdxLangTree(list));
const requestEventById = async () => {
  if (get(requestTextStorageId) <= 0) {
    void MessagePlugin.warning(t("msg.idInvalid"));
    return;
  }
  try {
    setIdFetchLoading(true);
    const d = await getPdxLangParsedEventItemById(get(requestTextStorageId));
    set(eventResult, d);
    set(eventSelId, 0);
    set(resultTree, []);
    setMenuUploadVisible(false);
  } catch (e) {
    console.error(e);
  } finally {
    setIdFetchLoading(false);
  }
};
const requestItemById = async () => {
  if (get(requestTextStorageId) <= 0) {
    void MessagePlugin.warning(t("msg.idInvalid"));
    return;
  }
  try {
    setIdFetchLoading(true);
    const d = await getPdxLangParsedItemById(get(requestTextStorageId));
    parse(d);
    set(eventResult, []);
    setMenuUploadVisible(false);
  } catch (e) {
    console.error(e);
  } finally {
    setIdFetchLoading(false);
  }
};
const requestMethod = async (file: UploadFile): Promise<RequestMethodResponse> => {
  try {
    setFileUploading(true);
    switch (get(parseOption)) {
      case "tree": {
        const { data } = await apiPostWithFile<PdxLangItem[]>(
          "/api/pdx/parse/lang",
          new Blob([file.raw!], { type: file.type }),
        );

        parse(data);
        set(eventResult, []);

        break;
      }
      case "event": {
        const { data } = await apiPostWithFile<PdxLangEventItem[]>(
          "/api/pdx/parse/event",
          new Blob([file.raw!], { type: file.type }),
        );

        set(eventResult, data);
        set(eventSelId, 0);
        set(resultTree, []);

        break;
      }
      default: {
        void MessagePlugin.warning("Not Implemented");
        break;
      }
    }

    setMenuUploadVisible(false);

    return { status: "success", response: { success: true } };
  } catch (e) {
    console.error(e);
    return { status: "fail", error: "x", response: {} };
  } finally {
    setFileUploading(false);
  }
};
const handleStickyTool = (context: { e: MouseEvent; item: TdStickyItemProps }) => {
  const { label } = context.item;
  switch (label) {
    case "Replace":
      setMenuReplaceVisible();
      break;
    case "Event":
      setEventDrawerVisible();
      break;
    case "Event+":
      handleEventPlus();
      break;
    case "Event-":
      handleEventMinus();
      break;
  }
};
const moveStickTool = () => {
  if (stickToolProp.value.placement === "left-bottom") {
    stickToolProp.value.placement = "right-bottom";
  } else {
    stickToolProp.value.placement = "left-bottom";
  }
};
const scrollTop = () => {
  if (baseLayoutEl) {
    baseLayoutEl.scrollTop = 0;
  }
};
const handleSwitchEvent = (id: number) => {
  if (id < 0 || id >= eventResult.value.length) {
    return;
  }
  scrollTop();
  set(eventSelId, id);
  setEventDrawerVisible(false);
};
const handleEventPlus = () => {
  if (eventMode.value && eventSelId.value < eventResult.value.length - 1) {
    scrollTop();
    eventSelId.value++;
  }
};
const handleEventMinus = () => {
  if (eventMode.value && eventSelId.value > 0) {
    scrollTop();
    eventSelId.value--;
  }
};

onKeyStroke("ArrowUp", handleEventMinus);
onKeyStroke("ArrowLeft", handleEventMinus);
onKeyStroke("ArrowDown", handleEventPlus);
onKeyStroke("ArrowRight", handleEventPlus);

onMounted(() => {
  if (!get(initialized)) {
    set(initialized, true);
  }
});
</script>

<template>
  <content-layout :title="t('title')">
    <div v-if="treeMode" class="r-pdx-container">
      <t-layout class="r-pdx-layout">
        <t-content>
          <t-card class="r-pdx-card">
            <t-space class="break-words" direction="vertical">
              <div v-for="(item, index) in treeSel" :key="index" v-html="textRender(item)" />
            </t-space>
          </t-card>
        </t-content>
        <t-aside :width="sideWidth">
          <t-card :header-bordered="true" title="Tree">
            <t-tree
              v-model:actived="treeVal"
              :activable="true"
              :data="resultTree"
              :hover="true"
              :line="true"
              :transition="true"
              class="r-pdx-tree"
            />
          </t-card>
        </t-aside>
      </t-layout>
    </div>
    <div v-else-if="eventMode" class="mb-20 w-full">
      <t-space class="w-full" direction="vertical">
        <t-card
          v-if="eventSel"
          :id="`pdx-event-${eventSelId}`"
          :key="eventSelId"
          :header-bordered="true"
          :title="textAlias(eventSel.name)"
          class="w-full"
        >
          <t-space class="w-full" direction="vertical">
            <div v-for="(line, lineKey) in sepTextContent(eventSel.desc)" :key="lineKey" v-html="textRender(line)" />
            <t-space class="m-auto w-full" direction="vertical">
              <t-card
                v-for="(opt, optKey) in eventSel.options"
                :key="optKey"
                :title="opt.showResp ? textAlias(opt.name) : undefined"
                class="r-no-select m-auto w-full"
                size="small"
                @click="
                  () => {
                    if (opt.resp.length > 0) opt.showResp = !opt.showResp;
                  }
                "
              >
                <t-tooltip :content="opt.tooltip.length > 0 ? opt.tooltip : undefined">
                  <t-space v-if="!opt.showResp" :size="4" class="text-center">
                    <InfoCircleIcon v-if="opt.resp.length > 0" size="16px" style="transform: translateY(-1px)" />
                    <t-text>{{ textAlias(opt.name) }}</t-text>
                  </t-space>
                  <div
                    v-for="(respLine, respKey) in sepTextContent(opt.resp)"
                    v-else
                    :key="respKey"
                    v-html="textRender(respLine)"
                  />
                </t-tooltip>
              </t-card>
            </t-space>
          </t-space>
          <template #actions>
            <span class="r-no-select font-mono">{{ eventSelId }}</span>
          </template>
        </t-card>
      </t-space>
    </div>
    <div v-else class="mt-6">
      <t-empty :description="t('empty.description')" :title="t('empty.title')" />
    </div>

    <!-- TopBar Actions -->
    <template #actions>
      <t-button shape="circle" theme="primary" variant="text" @click="setMenuReplaceVisible()">
        <RefreshIcon />
      </t-button>
      <t-button v-if="treeMode && narrow" shape="circle" theme="primary" variant="text" @click="setTreeDrawerVisible()">
        <ListIcon />
      </t-button>
      <t-button v-if="eventMode" shape="circle" theme="primary" variant="text" @click="moveStickTool">
        <Move1Icon />
      </t-button>
      <t-button shape="circle" theme="primary" variant="text" @click="setMenuUploadVisible()">
        <UploadIcon />
      </t-button>
      <RouterLink to="/">
        <t-button shape="circle" theme="primary" variant="text">
          <HomeIcon />
        </t-button>
      </RouterLink>
    </template>

    <!-- Sticky Tool -->
    <t-space>
      <t-sticky-tool
        :offset="stickToolProp.offset"
        :placement="stickToolProp.placement"
        shape="round"
        type="compact"
        @click="handleStickyTool"
      >
        <t-sticky-item label="Replace">
          <template #icon>
            <RefreshIcon />
          </template>
        </t-sticky-item>
        <template v-if="eventMode">
          <t-sticky-item label="Event">
            <template #icon>
              <ListIcon />
            </template>
          </t-sticky-item>
          <t-sticky-item label="Event-">
            <template #icon>
              <ArrowUpIcon />
            </template>
          </t-sticky-item>
          <t-sticky-item label="Event+">
            <template #icon>
              <ArrowDownIcon />
            </template>
          </t-sticky-item>
        </template>
      </t-sticky-tool>
    </t-space>

    <!-- Event Drawer -->
    <t-drawer v-model:visible="eventDrawerVisible" :footer="false">
      <t-list :scroll="{ type: 'virtual' }" :stripe="true" size="small">
        <t-list-item v-for="(event, eventKey) in eventResult" :key="eventKey">
          <span>{{ textAlias(event.name) }}</span>
          <template #action>
            <t-button
              :size="'extra-small' as SizeEnum"
              shape="circle"
              variant="outline"
              @click="handleSwitchEvent(eventKey)"
            >
              {{ eventKey }}
            </t-button>
          </template>
        </t-list-item>
      </t-list>
    </t-drawer>

    <!-- Tree Drawer (Only when narrow) -->
    <t-drawer v-model:visible="treeDrawerVisible" :footer="false">
      <t-card>
        <t-tree
          v-model:actived="treeVal"
          :activable="true"
          :data="resultTree"
          :hover="true"
          :line="true"
          :transition="true"
          @click="setTreeDrawerVisible(false)"
        />
      </t-card>
    </t-drawer>

    <!-- Upload Dialog -->
    <t-dialog v-model:visible="menuUploadVisible" :footer="false" :header="t('upload.title')" width="85%">
      <div class="w-full">
        <t-space v-if="!idFetchLoading" align="baseline" direction="vertical">
          <t-title level="h6">{{ t("upload.s1") }}</t-title>
          <t-radio-group v-model="parseOption" variant="default-filled">
            <t-radio-button label="树" value="tree" />
            <t-radio-button label="事件" value="event" />
          </t-radio-group>
          <t-upload
            :placeholder="t('upload.placeholder')"
            :request-method="requestMethod"
            :tips="t('upload.tips')"
            theme="file-input"
          >
            <t-button shape="circle" theme="default" variant="outline">
              <Upload1Icon />
            </t-button>
          </t-upload>
        </t-space>
        <template v-if="authPassed && !fileUploading">
          <t-divider v-if="!idFetchLoading" />
          <t-title level="h6">{{ t("upload.s2") }}</t-title>
          <t-form label-align="top">
            <t-form-item label="ID">
              <t-input v-model="requestTextStorageId" :auto-width="true" />
            </t-form-item>
            <t-form-item :label="t('upload.dbTree')">
              <t-button :loading="idFetchLoading" theme="primary" @click="requestItemById">
                <DownloadIcon v-if="!idFetchLoading" />
              </t-button>
            </t-form-item>
            <t-form-item :label="t('upload.dbEvent')">
              <t-button :loading="idFetchLoading" theme="primary" @click="requestEventById">
                <DownloadIcon v-if="!idFetchLoading" />
              </t-button>
            </t-form-item>
          </t-form>
        </template>
      </div>
    </t-dialog>

    <!-- Replace Dialog -->
    <t-dialog v-model:visible="menuReplaceVisible" :header="t('replace.title')" width="85%">
      <div v-if="!menuReplaceAddVisible">
        <t-list :split="true" :stripe="true" size="small" style="max-height: 280px">
          <t-list-item v-for="(value, key) in replacer" :key="key">
            <t-list-item-meta :description="value" :title="key.toString()" />
            <template #action>
              <t-button
                shape="circle"
                size="small"
                theme="warning"
                variant="outline"
                @click="store.removeReplacer(key.toString())"
              >
                <DeleteIcon />
              </t-button>
            </template>
          </t-list-item>
        </t-list>
      </div>
      <div v-else>
        <t-form label-align="top">
          <t-form-item :label="t('replace.key')">
            <t-input v-model="addReplacerKey" />
          </t-form-item>
          <t-form-item :label="t('replace.value')">
            <t-input v-model="addReplacerValue" />
          </t-form-item>
          <t-form-item :label="authPassed ? t('replace.modify') : t('replace.modifyOnly')">
            <t-space :size="6">
              <t-button shape="circle" @click="store.updateReplacer()">
                <AddIcon />
              </t-button>
              <t-button v-if="authPassed" shape="circle" @click="store.setSyncReplacer()">
                <UploadIcon />
              </t-button>
              <t-button v-if="authPassed" shape="circle" @click="store.getSyncReplacer()">
                <DownloadIcon />
              </t-button>
            </t-space>
          </t-form-item>
        </t-form>
      </div>
      <template #footer>
        <t-button shape="circle" theme="default" @click="setMenuReplaceAddVisible()">
          <SettingIcon />
        </t-button>
      </template>
    </t-dialog>
  </content-layout>
</template>

<style lang="less" scoped>
.r-pdx-container {
  max-height: 100%;
  max-width: 100%;
  width: 100%;

  .r-pdx-layout {
    background: var(--td-bg-color-container);

    .r-pdx-card {
      height: 67vh;
      max-height: 67vh;
      overflow: auto;
      scrollbar-width: thin;
    }

    .r-pdx-tree {
      max-height: 55.4vh;
      overflow: auto;
      scrollbar-width: thin;
    }
  }
}

.move-enter-active {
  transition: all 0.3s ease-out;
}

.move-leave-active {
  transition: all 0.3s cubic-bezier(1, 0.5, 0.8, 1);
}

.move-enter-from,
.move-leave-to {
  transform: translateX(-20px);
  opacity: 0;
}
</style>

<i18n locale="en">
title: PDX Parser
empty:
  title: No Content
  description: Click the upload button on the top right to parse the content
upload:
  title: Upload to Parse
  s1: File Upload
  s2: Database
  placeholder: Paradox Yaml Lang
  tips: Support YAML, do not upload the file repeatedly, your browser can't handle it
  dbTree: Parse Tree from Database
  dbEvent: Parse Event from Database
replace:
  title: Replace Value
  key: Key
  value: Value
  modify: Modify / Upload / Read
  modifyOnly: Modify
  sync: Sync
  download: Download
  upload: Upload
  tips: The key is not case sensitive
msg:
  idInvalid: ID Invalid
</i18n>

<i18n locale="zh-CN">
title: PDX 解析器
empty:
  title: 无内容
  description: 点击右上角上传按钮解析内容
upload:
  title: 上传解析源
  s1: 文件上传
  s2: 数据库
  placeholder: Paradox Yaml Lang
  tips: 支持 YAML，不要重复上传文件，你的浏览器受不了
  dbTree: 从数据库解析树
  dbEvent: 从数据库解析事件
replace:
  title: 替换值
  key: 键
  value: 值
  modify: 修改 / 上传 / 读取
  modifyOnly: 修改
  sync: 同步
  download: 下载
  upload: 上传
  tips: 键不区分大小写
msg:
  idInvalid: ID 无效
</i18n>
