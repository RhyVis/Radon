<script lang="ts" setup>
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
} from "@/pages/util/pdx-parser/scripts/function.ts";
import { usePdxStore } from "@/pages/util/pdx-parser/scripts/store";
import { useGlobalStore } from "@/store/global";
import { get, set, useDark, useToggle } from "@vueuse/core";
import { useRouteHash } from "@vueuse/router";
import { storeToRefs } from "pinia";
import {
  AddIcon,
  DeleteIcon,
  DownloadIcon,
  HomeIcon,
  ListIcon,
  RefreshIcon,
  SettingIcon,
  Upload1Icon,
  UploadIcon,
} from "tdesign-icons-vue-next";
import {
  Content as TContent,
  MessagePlugin,
  type RequestMethodResponse,
  type SizeEnum,
  type TdStickyItemProps,
  type TreeNodeValue,
  type TreeProps,
  type UploadFile,
} from "tdesign-vue-next";
import { computed, onMounted, ref, shallowRef } from "vue";

const hash = useRouteHash();
const global = useGlobalStore();
const store = usePdxStore();
const narrow = useNarrow();
const [eventDrawerVisible, setEventDrawerVisible] = useToggle(false);
const [treeDrawerVisible, setTreeDrawerVisible] = useToggle(false);
const [menuUploadVisible, setMenuUploadVisible] = useToggle(false);
const [menuReplaceVisible, setMenuReplaceVisible] = useToggle(false);
const [menuReplaceAddVisible, setMenuReplaceAddVisible] = useToggle(false);
const [idFetchLoading, setIdFetchLoading] = useToggle(false);
const { initialized, replacer, addReplacerKey, addReplacerValue } = storeToRefs(store);
const { authPassed } = storeToRefs(global);
const regQuote = /\\"/g;
const dark = useDark();
const requestTextStorageId = ref(0);
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

const eventResult = shallowRef<PdxLangEventItem[]>([]);
const resultTree = shallowRef<TreeProps["data"]>([]);
const sepTextContent = (raw: string) =>
  raw
    .replace(regQuote, '"')
    .replace(/(\\n)+/, "\\n")
    .split("\\n");
const textRender = (raw: string) => renderPdxColor(raw, get(replacer), get(dark));
const textAlias = (raw: string) => replacePdxAlias(raw, get(replacer));
const parse = (list: PdxLangItem[]) => set(resultTree, buildPdxLangTree(list));
const requestEventById = async () => {
  if (get(requestTextStorageId) <= 0) {
    void MessagePlugin.warning("ID 无效");
    return;
  }
  try {
    setIdFetchLoading(true);
    const d = await getPdxLangParsedEventItemById(get(requestTextStorageId));
    set(eventResult, d);
    setMenuUploadVisible(false);
  } catch (e) {
    console.error(e);
  } finally {
    setIdFetchLoading(false);
  }
};
const requestItemById = async () => {
  if (get(requestTextStorageId) <= 0) {
    void MessagePlugin.warning("ID 无效");
    return;
  }
  try {
    setIdFetchLoading(true);
    const d = await getPdxLangParsedItemById(get(requestTextStorageId));
    parse(d);
    setMenuUploadVisible(false);
  } catch (e) {
    console.error(e);
  } finally {
    setIdFetchLoading(false);
  }
};
const requestMethod = async (file: UploadFile): Promise<RequestMethodResponse> => {
  try {
    const { data } = await apiPostWithFile<PdxLangItem[]>(
      "/api/pdx/parse/lang",
      new Blob([file.raw!], { type: file.type }),
    );

    parse(data);
    setMenuUploadVisible(false);

    return { status: "success", response: { success: true } };
  } catch (e) {
    console.error(e);
    return { status: "fail", error: "失败", response: {} };
  }
};
const handleStickyTool = (context: { e: MouseEvent; item: TdStickyItemProps }) => {
  const { label } = context.item;
  if (label === "Replace") {
    setMenuReplaceVisible();
  } else if (label === "Event") {
    setEventDrawerVisible();
  }
};
const handleJumpToEvent = (id: number) => {
  const i = `pdx-event-${id}`;
  set(hash, i);
  const el = document.getElementById(i);
  if (el) {
    el.scrollIntoView({ behavior: "smooth" });
  }
  setEventDrawerVisible(false);
};

onMounted(() => {
  if (!get(initialized)) {
    set(initialized, true);
  }
});
</script>

<template>
  <content-layout title="PDX Parser">
    <div v-if="resultTree!.length > 0" class="r-pdx-container">
      <t-layout>
        <t-content>
          <t-card class="r-pdx-card">
            <t-space class="break-words" direction="vertical">
              <div v-for="(item, index) in treeSel" :key="index" v-html="textRender(item)" />
            </t-space>
          </t-card>
        </t-content>
        <t-aside :width="sideWidth">
          <t-card>
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
    <div v-else-if="eventResult.length > 0" class="w-full">
      <t-space class="w-full" direction="vertical">
        <t-card
          v-for="(event, eventKey) in eventResult"
          :id="`pdx-event-${eventKey}`"
          :key="eventKey"
          :header-bordered="true"
          :hover-shadow="true"
          :title="textAlias(event.name)"
          class="w-full"
        >
          <t-space class="w-full" direction="vertical">
            <div v-for="(line, lineKey) in sepTextContent(event.desc)" :key="lineKey" v-html="textRender(line)" />
            <t-space class="m-auto w-3/4" direction="vertical">
              <t-card
                v-for="(opt, optKey) in event.options"
                :key="optKey"
                class="r-no-select m-auto w-full"
                size="small"
              >
                <span class="text-center">{{ textAlias(opt.name) }}</span>
              </t-card>
            </t-space>
          </t-space>
          <template #actions>
            <span class="r-no-select">{{ eventKey }}</span>
          </template>
        </t-card>
      </t-space>
    </div>
    <div v-else class="mt-6">
      <t-empty />
    </div>

    <template #actions>
      <t-button shape="circle" theme="primary" variant="text" @click="setMenuReplaceVisible()">
        <RefreshIcon />
      </t-button>
      <t-button v-if="narrow" shape="circle" theme="primary" variant="text" @click="setTreeDrawerVisible()">
        <ListIcon />
      </t-button>
      <t-button
        v-if="eventResult.length > 0"
        shape="circle"
        theme="primary"
        variant="text"
        @click="setEventDrawerVisible()"
      >
        <ListIcon />
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
        :offset="[-50, 20]"
        placement="right-bottom"
        shape="round"
        type="compact"
        @click="handleStickyTool"
      >
        <t-sticky-item label="Replace">
          <template #icon>
            <RefreshIcon />
          </template>
        </t-sticky-item>
        <t-sticky-item v-if="eventResult.length > 0" label="Event">
          <template #icon>
            <ListIcon />
          </template>
        </t-sticky-item>
      </t-sticky-tool>
    </t-space>

    <!-- Event Drawer -->
    <t-drawer v-model:visible="eventDrawerVisible" :footer="false">
      <t-list :stripe="true" size="small">
        <t-list-item v-for="(event, eventKey) in eventResult" :key="eventKey">
          <span>{{ textAlias(event.name) }}</span>
          <template #action>
            <t-button
              :size="'extra-small' as SizeEnum"
              shape="round"
              variant="text"
              @click="handleJumpToEvent(eventKey)"
            >
              >
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
    <t-dialog v-model:visible="menuUploadVisible" :footer="false" header="上传" width="85%">
      <div class="w-full">
        <t-space v-if="!idFetchLoading" align="baseline" direction="vertical">
          <t-upload
            :request-method="requestMethod"
            placeholder="Paradox Yaml Lang"
            theme="file-input"
            tips="支持YAML，不要反复上传文件，你的浏览器把持不住"
          >
            <t-button shape="circle" theme="default" variant="outline">
              <Upload1Icon />
            </t-button>
          </t-upload>
        </t-space>
        <template v-if="authPassed">
          <t-divider v-if="!idFetchLoading" />
          <t-form label-align="top">
            <t-form-item label="ID">
              <t-input v-model="requestTextStorageId" :auto-width="true" />
            </t-form-item>
            <t-form-item label="从数据库加载全部">
              <t-button :loading="idFetchLoading" theme="primary" @click="requestItemById">
                <DownloadIcon v-if="!idFetchLoading" />
              </t-button>
            </t-form-item>
            <t-form-item label="从数据库加载事件">
              <t-button :loading="idFetchLoading" theme="primary" @click="requestEventById">
                <DownloadIcon v-if="!idFetchLoading" />
              </t-button>
            </t-form-item>
          </t-form>
        </template>
      </div>
    </t-dialog>

    <!-- Replace Dialog -->
    <t-dialog v-model:visible="menuReplaceVisible" header="替换值" width="85%">
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
          <t-form-item label="键">
            <t-input v-model="addReplacerKey" />
          </t-form-item>
          <t-form-item label="值">
            <t-input v-model="addReplacerValue" />
          </t-form-item>
          <t-form-item :label="authPassed ? '修改 / 上传 / 读取' : '修改'">
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
  max-height: 75vh;
  max-width: 100%;
  width: 100%;

  .r-pdx-card {
    height: 70.3vh;
    max-height: 70.3vh;
    overflow: auto;
    scrollbar-width: thin;
  }

  .r-pdx-tree {
    max-height: 66vh;
    overflow: auto;
    scrollbar-width: thin;
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
