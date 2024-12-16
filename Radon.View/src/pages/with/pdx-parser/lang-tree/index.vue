<script lang="ts" setup>
import { useNarrow } from "@/composable/useNarrow.ts";
import PpDrawerTree from "@/pages/with/pdx-parser/comp/PpDrawerTree.vue";
import PpReplacer from "@/pages/with/pdx-parser/comp/PpReplacer.vue";
import PdxParserUploadDialog from "@/pages/with/pdx-parser/comp/PpUploadDialog.vue";
import { usePdxTextRender } from "@/pages/with/pdx-parser/comp/usePdxTextRender.ts";
import "@/pages/with/pdx-parser/pdx-color.css";
import { sepTextContent } from "@/pages/with/pdx-parser/scripts/function.ts";
import { get, useToggle } from "@vueuse/core";
import { HomeIcon, ListIcon, RefreshIcon, UploadIcon } from "tdesign-icons-vue-next";
import { Content as TContent, type TreeNodeValue, type TreeProps } from "tdesign-vue-next";
import { computed, ref, shallowRef } from "vue";
import { useI18n } from "vue-i18n";

const { t } = useI18n();
const [treeDrawerVisible, setTreeDrawerVisible] = useToggle(false);
const [menuUploadVisible, setMenuUploadVisible] = useToggle(false);
const [menuReplaceVisible, setMenuReplaceVisible] = useToggle(false);

const narrow = useNarrow();
const treeResult = shallowRef<TreeProps["data"]>([]);
const treeActive = ref<TreeNodeValue[]>([]);
const treeSel = computed(() => {
  const val = treeActive.value[0];
  if (val) {
    return sepTextContent(val.toString());
  } else {
    return [];
  }
});
const sideWidth = computed(() => (get(narrow) ? "0" : "245px"));

const treeExist = computed(() => treeResult.value?.length ?? -1 > 0);

const { textRender } = usePdxTextRender();
</script>

<template>
  <content-layout :title="t('title')">
    <div v-if="treeExist" class="r-pdx-container">
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
              v-model:actived="treeActive"
              :activable="true"
              :data="treeResult"
              :hover="true"
              :line="true"
              :transition="true"
              class="r-pdx-tree"
            />
          </t-card>
        </t-aside>
      </t-layout>
    </div>
    <div v-else class="mt-6">
      <t-empty :description="t('empty.description')" :title="t('empty.title')" />
    </div>

    <!-- TopBar Actions -->
    <template #actions>
      <t-button shape="circle" theme="primary" variant="text" @click="setMenuReplaceVisible()">
        <RefreshIcon />
      </t-button>
      <t-button v-if="narrow" shape="circle" theme="primary" variant="text" @click="setTreeDrawerVisible()">
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

    <!-- Tree Drawer (Only when narrow) -->
    <PpDrawerTree v-model:visible="treeDrawerVisible" :result-tree="treeResult" :tree-val="treeActive" />

    <!-- Upload Dialog -->
    <PdxParserUploadDialog
      v-model:menu-upload-visible="menuUploadVisible"
      v-model:parse-result-tree="treeResult"
      type="tree"
    />

    <!-- Replace Dialog -->
    <PpReplacer v-model:visible="menuReplaceVisible" />
  </content-layout>
</template>

<style scoped>
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
</style>

<i18n locale="en">
title: PDX Tree Parser
empty:
  title: No Content
  description: Click the upload button on the top right to parse the content
</i18n>

<i18n locale="zh-CN">
title: PDX 树解析器
empty:
  title: 无内容
  description: 点击右上角上传按钮解析内容
</i18n>
